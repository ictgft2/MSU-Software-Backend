using System.Text;
using System.Text.Json;
using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Gilead.Domain.Entities;
using Gilead.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Gilead.Application.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddGileadServices(this IServiceCollection services)
    {
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IEncounterService, EncounterService>();
        services.AddScoped<IQueueService, QueueService>();
        services.AddScoped<IVitalsService, VitalsService>();
        services.AddScoped<IConsultationService, ConsultationService>();
        services.AddScoped<ILabService, LabService>();
        services.AddScoped<IDressingService, DressingService>();
        services.AddScoped<IPharmacyService, PharmacyService>();
        services.AddScoped<IProtocolService, ProtocolService>();
        services.AddScoped<IContactTraceService, ContactTraceService>();
        services.AddScoped<IRegisterService, RegisterService>();
        services.AddScoped<IServiceWindowService, ServiceWindowService>();
        return services;
    }
}

internal sealed class PatientService(IPatientRepository patients) : IPatientService
{
    public async Task<ServiceResult<Patient>> RegisterAsync(RegisterPatientRequest request, CancellationToken cancellationToken)
    {
        if (request.Age < 0 || request.Sex is not ("M" or "F"))
            return ServiceResult<Patient>.Fail("Invalid patient demographics.");

        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Age = request.Age,
            Sex = request.Sex,
            Phone = request.Phone,
            Address = request.Address,
            NextOfKinName = request.NextOfKinName,
            NextOfKinPhone = request.NextOfKinPhone,
            NextOfKinRelationship = request.NextOfKinRelationship,
            CreatedAt = DateTimeOffset.UtcNow
        };

        return ServiceResult<Patient>.Ok(await patients.InsertAsync(patient, cancellationToken), 201);
    }

    public async Task<ServiceResult<Patient>> GetByIdAsync(Guid patientId, CancellationToken cancellationToken)
    {
        var patient = await patients.GetByIdAsync(patientId, cancellationToken);
        return patient is null ? ServiceResult<Patient>.Fail("Patient not found.", 404) : ServiceResult<Patient>.Ok(patient);
    }

    public async Task<ServiceResult<IReadOnlyList<Patient>>> SearchAsync(string? name, string? phone, CancellationToken cancellationToken) =>
        ServiceResult<IReadOnlyList<Patient>>.Ok(await patients.SearchAsync(name, phone, cancellationToken));
}

internal sealed class EncounterService(IEncounterRepository encounters, IPatientRepository patients, IServiceWindowRepository windows, IQueueCacheService queue) : IEncounterService
{
    public async Task<ServiceResult<Encounter>> OpenAsync(OpenEncounterRequest request, CancellationToken cancellationToken)
    {
        var patient = await patients.GetByIdAsync(request.PatientId, cancellationToken);
        if (patient is null)
            return ServiceResult<Encounter>.Fail("Patient not found.", 404);

        if (request.AdmissionType == AdmissionType.ColdCase && !await IsColdCaseOpenAsync(cancellationToken))
            return ServiceResult<Encounter>.Fail("Cold case intake is closed.", 409);

        var now = DateTimeOffset.UtcNow;
        var status = request.AdmissionType == AdmissionType.Emergency
            ? EncounterStatus.Admitted
            : patient.Age > 40 ? EncounterStatus.BpCheck : EncounterStatus.Queued;

        var encounter = new Encounter
        {
            Id = Guid.NewGuid(),
            PatientId = request.PatientId,
            AdmissionType = request.AdmissionType,
            ArrivalMode = request.ArrivalMode,
            ChiefComplaint = request.ChiefComplaint,
            RegisteredBy = request.RegisteredBy,
            Status = status,
            AdmittedAt = now,
            CreatedAt = now,
            UpdatedAt = now
        };

        var created = await encounters.InsertAsync(encounter, cancellationToken);
        if (created.AdmissionType == AdmissionType.ColdCase && created.Status == EncounterStatus.Queued)
            await queue.JoinAsync(created.Id, DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);

        return ServiceResult<Encounter>.Ok(created, 201);
    }

    public async Task<ServiceResult<EncounterDetail>> GetDetailAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        var detail = await encounters.GetDetailAsync(encounterId, cancellationToken);
        return detail is null ? ServiceResult<EncounterDetail>.Fail("Encounter not found.", 404) : ServiceResult<EncounterDetail>.Ok(detail);
    }

    public async Task<ServiceResult<IReadOnlyList<Encounter>>> ListAsync(EncounterStatus? status, DateOnly? date, AdmissionType? type, CancellationToken cancellationToken) =>
        ServiceResult<IReadOnlyList<Encounter>>.Ok(await encounters.GetListAsync(status, date, type, cancellationToken));

    public async Task<ServiceResult> AdvanceStatusAsync(Guid encounterId, EncounterStatus status, CancellationToken cancellationToken)
    {
        var encounter = await encounters.GetByIdAsync(encounterId, cancellationToken);
        if (encounter is null)
            return ServiceResult.Fail("Encounter not found.", 404);

        if (!IsValidTransition(encounter.Status, status))
            return ServiceResult.Fail($"Invalid status transition from {encounter.Status} to {status}.", 409);

        await encounters.UpdateStatusAsync(encounterId, status, status is EncounterStatus.Discharged or EncounterStatus.Referred ? DateTimeOffset.UtcNow : null, cancellationToken);
        return ServiceResult.Ok();
    }

    private async Task<bool> IsColdCaseOpenAsync(CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var current = await windows.GetCurrentAsync(today, cancellationToken);
        if (current is null)
            return false;

        var now = TimeOnly.FromDateTime(DateTime.Now);
        return now >= current.ColdCaseOpenTime && now <= current.ColdCaseCloseTime;
    }

    private static bool IsValidTransition(EncounterStatus current, EncounterStatus next) =>
        (current, next) switch
        {
            (EncounterStatus.Admitted, EncounterStatus.InTreatment) => true,
            (EncounterStatus.InTreatment, EncounterStatus.Discharged or EncounterStatus.Referred) => true,
            (EncounterStatus.Registered, EncounterStatus.BpCheck or EncounterStatus.Queued) => true,
            (EncounterStatus.BpCheck, EncounterStatus.Queued) => true,
            (EncounterStatus.Queued, EncounterStatus.InConsultation) => true,
            (EncounterStatus.InConsultation, EncounterStatus.PharmacyPending or EncounterStatus.LabPending or EncounterStatus.DressingPending or EncounterStatus.AwaitingHandover or EncounterStatus.Referred) => true,
            (EncounterStatus.PharmacyPending or EncounterStatus.LabPending or EncounterStatus.DressingPending, EncounterStatus.AwaitingHandover) => true,
            (EncounterStatus.AwaitingHandover, EncounterStatus.Discharged or EncounterStatus.Referred) => true,
            _ => current == next
        };
}

internal sealed class QueueService(IEncounterRepository encounters, IQueueCacheService queue) : IQueueService
{
    private static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);

    public async Task<ServiceResult> JoinAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        var encounter = await encounters.GetByIdAsync(encounterId, cancellationToken);
        if (encounter is null)
            return ServiceResult.Fail("Encounter not found.", 404);
        if (encounter.AdmissionType == AdmissionType.Emergency)
            return ServiceResult.Fail("Emergency encounters do not join the cold case queue.", 409);

        await queue.JoinAsync(encounterId, Today, cancellationToken);
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DequeueAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        await queue.DequeueAsync(encounterId, Today, cancellationToken);
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult<long>> GetPositionAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        var position = await queue.GetPositionAsync(encounterId, Today, cancellationToken);
        return position is null ? ServiceResult<long>.Fail("Encounter is not in queue.", 404) : ServiceResult<long>.Ok(position.Value);
    }

    public async Task<ServiceResult<IReadOnlyList<QueueEntry>>> GetFullListAsync(CancellationToken cancellationToken) =>
        ServiceResult<IReadOnlyList<QueueEntry>>.Ok(await queue.GetFullListAsync(Today, cancellationToken));
}

internal sealed class VitalsService(IVitalsRepository vitals, IEncounterRepository encounters, IPatientRepository patients, IQueueCacheService queue) : IVitalsService
{
    public async Task<ServiceResult<VitalSigns>> RecordAsync(Guid encounterId, RecordVitalsRequest request, CancellationToken cancellationToken)
    {
        var encounter = await encounters.GetByIdAsync(encounterId, cancellationToken);
        if (encounter is null)
            return ServiceResult<VitalSigns>.Fail("Encounter not found.", 404);

        var reading = new VitalSigns
        {
            Id = Guid.NewGuid(),
            EncounterId = encounterId,
            RecordedBy = request.RecordedBy,
            BloodPressureSystolic = request.BloodPressureSystolic,
            BloodPressureDiastolic = request.BloodPressureDiastolic,
            PulseRate = request.PulseRate,
            Temperature = request.Temperature,
            Spo2 = request.Spo2,
            RespiratoryRate = request.RespiratoryRate,
            Weight = request.Weight,
            Notes = request.Notes,
            RecordedAt = DateTimeOffset.UtcNow
        };

        var created = await vitals.InsertAsync(reading, cancellationToken);
        if (encounter.AdmissionType == AdmissionType.ColdCase && encounter.Status == EncounterStatus.BpCheck)
        {
            var patient = await patients.GetByIdAsync(encounter.PatientId, cancellationToken);
            if (patient?.Age > 40)
            {
                await encounters.UpdateStatusAsync(encounterId, EncounterStatus.Queued, null, cancellationToken);
                await queue.JoinAsync(encounterId, DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);
            }
        }

        return ServiceResult<VitalSigns>.Ok(created, 201);
    }

    public async Task<ServiceResult<IReadOnlyList<VitalSigns>>> GetByEncounterAsync(Guid encounterId, CancellationToken cancellationToken) =>
        ServiceResult<IReadOnlyList<VitalSigns>>.Ok(await vitals.GetByEncounterAsync(encounterId, cancellationToken));

    public async Task<ServiceResult<VitalSigns>> GetLatestAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        var latest = await vitals.GetLatestAsync(encounterId, cancellationToken);
        return latest is null ? ServiceResult<VitalSigns>.Fail("No vitals found.", 404) : ServiceResult<VitalSigns>.Ok(latest);
    }
}

internal sealed class ConsultationService(IConsultationRepository consultations) : IConsultationService
{
    public async Task<ServiceResult<ConsultationNote>> SubmitAsync(Guid encounterId, SubmitConsultationRequest request, CancellationToken cancellationToken)
    {
        if (await consultations.GetByEncounterAsync(encounterId, cancellationToken) is not null)
            return ServiceResult<ConsultationNote>.Fail("Consultation already exists for this encounter.", 409);

        var plan = request.TreatmentPlan;
        var note = new ConsultationNote
        {
            Id = Guid.NewGuid(),
            EncounterId = encounterId,
            DoctorId = request.DoctorId,
            Diagnosis = JsonSerializer.Serialize(request.Diagnosis),
            ClinicalNotes = request.ClinicalNotes,
            RequiresLab = plan.LabTests.Count > 0,
            RequiresDressing = plan.RequiresDressing,
            IsReferral = plan.IsReferral,
            ReferralFacility = plan.ReferralFacility,
            ReferralReason = plan.ReferralReason,
            ConsultedAt = DateTimeOffset.UtcNow
        };

        var prescriptions = plan.Prescriptions.Select(p => new Prescription
        {
            Id = Guid.NewGuid(),
            ConsultationNoteId = note.Id,
            EncounterId = encounterId,
            DrugName = p.DrugName,
            Dosage = p.Dosage,
            Frequency = p.Frequency,
            Duration = p.Duration,
            Route = p.Route,
            Instructions = p.Instructions,
            Status = PrescriptionStatus.Pending,
            IssuedAt = DateTimeOffset.UtcNow
        }).ToArray();

        var labRequests = plan.LabTests.Select(t => new LabRequest
        {
            Id = Guid.NewGuid(),
            ConsultationNoteId = note.Id,
            EncounterId = encounterId,
            TestName = t.TestName,
            ClinicalIndication = t.ClinicalIndication,
            Status = LabRequestStatus.Pending,
            RequestedAt = DateTimeOffset.UtcNow
        }).ToArray();

        var dressing = plan.RequiresDressing
            ? new DressingOrder { Id = Guid.NewGuid(), ConsultationNoteId = note.Id, EncounterId = encounterId, Instructions = plan.DressingInstructions ?? string.Empty, Status = DressingOrderStatus.Pending, CreatedAt = DateTimeOffset.UtcNow }
            : null;

        var nextStatus = prescriptions.Length > 0 ? EncounterStatus.PharmacyPending
            : labRequests.Length > 0 ? EncounterStatus.LabPending
            : dressing is not null ? EncounterStatus.DressingPending
            : plan.IsReferral ? EncounterStatus.Referred : EncounterStatus.AwaitingHandover;

        await consultations.CreateWithChildrenAsync(note, prescriptions, labRequests, dressing, nextStatus, cancellationToken);
        return ServiceResult<ConsultationNote>.Ok(note, 201);
    }

    public async Task<ServiceResult<ConsultationNote>> GetByEncounterAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        var note = await consultations.GetByEncounterAsync(encounterId, cancellationToken);
        return note is null ? ServiceResult<ConsultationNote>.Fail("Consultation not found.", 404) : ServiceResult<ConsultationNote>.Ok(note);
    }
}

internal sealed class LabService(ILabRepository labs) : ILabService
{
    public async Task<ServiceResult<IReadOnlyList<LabRequest>>> GetRequestsAsync(LabRequestStatus? status, DateOnly? date, CancellationToken cancellationToken) => ServiceResult<IReadOnlyList<LabRequest>>.Ok(await labs.GetRequestsAsync(status, date, cancellationToken));
    public async Task<ServiceResult<LabRequest>> GetRequestAsync(Guid requestId, CancellationToken cancellationToken) => (await labs.GetRequestAsync(requestId, cancellationToken)) is { } r ? ServiceResult<LabRequest>.Ok(r) : ServiceResult<LabRequest>.Fail("Lab request not found.", 404);
    public async Task<ServiceResult<LabResult>> PostResultAsync(Guid requestId, PostLabResultRequest request, CancellationToken cancellationToken) => ServiceResult<LabResult>.Ok(await labs.InsertResultAsync(new LabResult { Id = Guid.NewGuid(), LabRequestId = requestId, ScientistId = request.ScientistId, TestName = request.TestName, Findings = request.Findings, Values = JsonSerializer.Serialize(request.Values), Conclusion = request.Conclusion, CompletedAt = DateTimeOffset.UtcNow }, cancellationToken), 201);
    public async Task<ServiceResult<IReadOnlyList<LabResult>>> GetResultsByEncounterAsync(Guid encounterId, CancellationToken cancellationToken) => ServiceResult<IReadOnlyList<LabResult>>.Ok(await labs.GetResultsByEncounterAsync(encounterId, cancellationToken));
}

internal sealed class DressingService(IDressingRepository dressing) : IDressingService
{
    public async Task<ServiceResult<IReadOnlyList<DressingOrder>>> GetWorklistAsync(DressingOrderStatus? status, CancellationToken cancellationToken) => ServiceResult<IReadOnlyList<DressingOrder>>.Ok(await dressing.GetWorklistAsync(status, cancellationToken));
    public async Task<ServiceResult<DressingOrder>> GetByIdAsync(Guid orderId, CancellationToken cancellationToken) => (await dressing.GetByIdAsync(orderId, cancellationToken)) is { } o ? ServiceResult<DressingOrder>.Ok(o) : ServiceResult<DressingOrder>.Fail("Dressing order not found.", 404);
    public async Task<ServiceResult> CompleteAsync(Guid orderId, CompleteDressingOrderRequest request, CancellationToken cancellationToken) { await dressing.CompleteAsync(orderId, request.PerformedBy, request.ProcedureNotes, cancellationToken); return ServiceResult.Ok(); }
}

internal sealed class PharmacyService(IPrescriptionRepository prescriptions, IDispensingRepository dispensings, IDrugHandoverRepository handovers) : IPharmacyService
{
    public async Task<ServiceResult<IReadOnlyList<Prescription>>> GetWorklistAsync(PrescriptionStatus? status, DateOnly? date, CancellationToken cancellationToken) => ServiceResult<IReadOnlyList<Prescription>>.Ok(await prescriptions.GetWorklistAsync(status, date, cancellationToken));
    public async Task<ServiceResult<Prescription>> GetByIdAsync(Guid id, CancellationToken cancellationToken) => (await prescriptions.GetByIdAsync(id, cancellationToken)) is { } p ? ServiceResult<Prescription>.Ok(p) : ServiceResult<Prescription>.Fail("Prescription not found.", 404);

    public async Task<ServiceResult<Dispensing>> DispenseAsync(Guid id, DispensePrescriptionRequest request, CancellationToken cancellationToken)
    {
        var prescription = await prescriptions.GetByIdAsync(id, cancellationToken);
        if (prescription is null)
            return ServiceResult<Dispensing>.Fail("Prescription not found.", 404);

        var dispensing = await dispensings.InsertAsync(new Dispensing { Id = Guid.NewGuid(), PrescriptionId = id, PharmacistId = request.PharmacistId, DrugName = prescription.DrugName, QuantityDispensed = request.QuantityDispensed, BatchNumber = request.BatchNumber, ExpiryDate = request.ExpiryDate, Notes = request.Notes, DispensedAt = DateTimeOffset.UtcNow }, cancellationToken);
        await prescriptions.UpdateStatusAsync(id, PrescriptionStatus.Dispensed, cancellationToken);
        await handovers.InsertAsync(new DrugHandover { Id = Guid.NewGuid(), DispensingId = dispensing.Id, EncounterId = prescription.EncounterId }, cancellationToken);
        return ServiceResult<Dispensing>.Ok(dispensing, 201);
    }
}

internal sealed class ProtocolService(IDrugHandoverRepository handovers, IDispensingRepository dispensings, IPrescriptionRepository prescriptions, IEncounterRepository encounters) : IProtocolService
{
    public async Task<ServiceResult<IReadOnlyList<DrugHandover>>> GetWorklistAsync(string? status, CancellationToken cancellationToken) => ServiceResult<IReadOnlyList<DrugHandover>>.Ok(await handovers.GetWorklistAsync(status, cancellationToken));
    public async Task<ServiceResult<DrugHandover>> GetByIdAsync(Guid handoverId, CancellationToken cancellationToken) => (await handovers.GetByIdAsync(handoverId, cancellationToken)) is { } h ? ServiceResult<DrugHandover>.Ok(h) : ServiceResult<DrugHandover>.Fail("Handover not found.", 404);

    public async Task<ServiceResult> ConfirmAsync(Guid handoverId, ConfirmHandoverRequest request, CancellationToken cancellationToken)
    {
        var handover = await handovers.GetByIdAsync(handoverId, cancellationToken);
        if (handover is null)
            return ServiceResult.Fail("Handover not found.", 404);
        var dispensing = await dispensings.GetByIdAsync(handover.DispensingId, cancellationToken);
        if (dispensing is null)
            return ServiceResult.Fail("Dispensing not found.", 404);

        handover.ProtocolOfficerId = request.ProtocolOfficerId;
        handover.PatientNameVerified = request.PatientNameVerified;
        handover.DrugListVerified = request.DrugListVerified;
        handover.DosageCounsellingDone = request.DosageCounsellingDone;
        handover.DurationCounsellingDone = request.DurationCounsellingDone;
        handover.CounsellingNotes = request.CounsellingNotes;
        handover.HandoverAt = DateTimeOffset.UtcNow;
        await handovers.ConfirmAsync(handover, cancellationToken);
        await prescriptions.UpdateStatusAsync(dispensing.PrescriptionId, PrescriptionStatus.HandedOver, cancellationToken);
        if (await prescriptions.AllHandedOverForEncounterAsync(handover.EncounterId, cancellationToken))
            await encounters.UpdateStatusAsync(handover.EncounterId, EncounterStatus.Discharged, DateTimeOffset.UtcNow, cancellationToken);
        return ServiceResult.Ok();
    }
}

internal sealed class ContactTraceService(IContactTraceRepository contactTraces) : IContactTraceService
{
    public async Task<ServiceResult<ContactTrace>> RecordAsync(Guid encounterId, ContactTraceRequest request, CancellationToken cancellationToken) => ServiceResult<ContactTrace>.Ok(await contactTraces.InsertAsync(ToEntity(encounterId, request, Guid.NewGuid()), cancellationToken), 201);
    public async Task<ServiceResult<ContactTrace>> GetAsync(Guid encounterId, CancellationToken cancellationToken) => (await contactTraces.GetByEncounterAsync(encounterId, cancellationToken)) is { } c ? ServiceResult<ContactTrace>.Ok(c) : ServiceResult<ContactTrace>.Fail("Contact trace not found.", 404);
    public async Task<ServiceResult<ContactTrace>> UpdateAsync(Guid encounterId, ContactTraceRequest request, CancellationToken cancellationToken)
    {
        var existing = await contactTraces.GetByEncounterAsync(encounterId, cancellationToken);
        if (existing is null)
            return ServiceResult<ContactTrace>.Fail("Contact trace not found.", 404);
        return ServiceResult<ContactTrace>.Ok(await contactTraces.UpdateAsync(ToEntity(encounterId, request, existing.Id), cancellationToken));
    }

    private static ContactTrace ToEntity(Guid encounterId, ContactTraceRequest request, Guid id) => new() { Id = id, EncounterId = encounterId, RecordedBy = request.RecordedBy, NextOfKinName = request.NextOfKinName, NextOfKinPhone = request.NextOfKinPhone, NextOfKinRelationship = request.NextOfKinRelationship, ResidentialAddress = request.ResidentialAddress, WorkplaceAddress = request.WorkplaceAddress, DischargeNotes = request.DischargeNotes, ReferralDestination = request.ReferralDestination, RecordedAt = DateTimeOffset.UtcNow };
}

internal sealed class RegisterService(IRegisterRepository register) : IRegisterService
{
    public async Task<ServiceResult<IReadOnlyList<DrugRegisterEntry>>> GetDrugsAsync(DateOnly? date, int page, int limit, CancellationToken cancellationToken) => ServiceResult<IReadOnlyList<DrugRegisterEntry>>.Ok(await register.GetDrugsAsync(date, Math.Max(page, 1), Math.Clamp(limit, 1, 200), cancellationToken));
    public async Task<ServiceResult<string>> ExportDrugsAsync(DateOnly? date, string? format, CancellationToken cancellationToken)
    {
        if (!string.Equals(format ?? "csv", "csv", StringComparison.OrdinalIgnoreCase))
            return ServiceResult<string>.Fail("Only csv export is supported.", 400);
        var rows = await register.ExportDrugsAsync(date, cancellationToken);
        var csv = new StringBuilder("HandoverId,EncounterId,PrescriptionId,DrugName,Dosage,Frequency,Duration,BatchNumber,QuantityDispensed,ExpiryDate,HandoverAt\n");
        foreach (var r in rows)
            csv.AppendLine($"{r.HandoverId},{r.EncounterId},{r.PrescriptionId},\"{r.DrugName}\",\"{r.Dosage}\",\"{r.Frequency}\",\"{r.Duration}\",\"{r.BatchNumber}\",{r.QuantityDispensed},{r.ExpiryDate:yyyy-MM-dd},{r.HandoverAt:O}");
        return ServiceResult<string>.Ok(csv.ToString());
    }
}

internal sealed class ServiceWindowService(IServiceWindowRepository windows) : IServiceWindowService
{
    public async Task<ServiceResult<ServiceWindowStatus>> GetCurrentAsync(CancellationToken cancellationToken)
    {
        var window = await windows.GetCurrentAsync(DateOnly.FromDateTime(DateTime.Now), cancellationToken);
        var now = TimeOnly.FromDateTime(DateTime.Now);
        return ServiceResult<ServiceWindowStatus>.Ok(new ServiceWindowStatus(window is not null && now >= window.ColdCaseOpenTime && now <= window.ColdCaseCloseTime, window));
    }

    public async Task<ServiceResult<ServiceTimeWindow>> SetTodayAsync(SetServiceWindowRequest request, CancellationToken cancellationToken) =>
        ServiceResult<ServiceTimeWindow>.Ok(await windows.InsertAsync(new ServiceTimeWindow { Id = Guid.NewGuid(), Date = request.Date, ColdCaseOpenTime = request.ColdCaseOpenTime, ColdCaseCloseTime = request.ColdCaseCloseTime, CreatedBy = request.CreatedBy, CreatedAt = DateTimeOffset.UtcNow }, cancellationToken), 201);

    public async Task<ServiceResult<ServiceTimeWindow>> UpdateAsync(Guid windowId, UpdateServiceWindowRequest request, CancellationToken cancellationToken) =>
        ServiceResult<ServiceTimeWindow>.Ok(await windows.UpdateAsync(windowId, request.ColdCaseOpenTime, request.ColdCaseCloseTime, cancellationToken));
}
