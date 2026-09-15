using System.Data;
using System.Text.Json;
using Dapper;
using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Gilead.Domain.Entities;
using Gilead.Domain.Enums;
using Gilead.Infrastructure.Data;

namespace Gilead.Infrastructure.Repositories;

internal static class Db
{
    public static DynamicParameters Params(params (string Name, object? Value)[] values)
    {
        var parameters = new DynamicParameters();
        foreach (var (name, value) in values)
        {
            parameters.Add(name, value);
        }

        return parameters;
    }

    public static string Function(string name, params string[] parameterNames) =>
        $"SELECT * FROM public.{name}({string.Join(", ", parameterNames.Select(name => $"@{name}"))});";

    public static CommandDefinition Command(
        string sql,
        object? parameters,
        CancellationToken cancellationToken,
        IDbTransaction? transaction = null) =>
        new(sql, parameters, transaction, cancellationToken: cancellationToken);

    public static string? S<T>(T? value) where T : struct, Enum => value?.ToString();
    public static DateTime? D(DateOnly? value) => value?.ToDateTime(TimeOnly.MinValue);
    public static TimeSpan T(TimeOnly value) => value.ToTimeSpan();
}

public sealed class StaffRepository(PostgresConnectionFactory factory) : IStaffRepository
{
    public async Task<Staff> InsertAsync(Staff staff, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Staff_Insert", "Id", "FullName", "Email", "Role", "IsActive", "CreatedAt");
        return await connection.QuerySingleAsync<Staff>(Db.Command(sql, Db.Params(
            ("Id", staff.Id),
            ("FullName", staff.FullName),
            ("Email", staff.Email),
            ("Role", staff.Role.ToString()),
            ("IsActive", staff.IsActive),
            ("PasswordHash", staff.PasswordHash),
            ("CreatedAt", staff.CreatedAt)), cancellationToken));
    }

    public async Task<Staff?> GetByIdAsync(Guid staffId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Staff_GetById", "StaffId");
        return await connection.QuerySingleOrDefaultAsync<Staff>(Db.Command(sql, Db.Params(("StaffId", staffId)), cancellationToken));
    }

    public async Task<Staff?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Staff_GetByEmail", "Email");
        return await connection.QuerySingleOrDefaultAsync<Staff>(Db.Command(sql, Db.Params(("Email", email)), cancellationToken));
    }

    public async Task<IReadOnlyList<Staff>> GetListAsync(StaffRole? role, bool? isActive, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Staff_GetList", "Role", "IsActive");
        var rows = await connection.QueryAsync<Staff>(Db.Command(sql, Db.Params(
            ("Role", role?.ToString()),
            ("IsActive", isActive)), cancellationToken));
        return rows.ToArray();
    }

    public async Task<Staff?> UpdateAsync(Staff staff, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Staff_Update", "Id", "FullName", "Email", "Role", "IsActive", "PasswordHash");
        return await connection.QuerySingleOrDefaultAsync<Staff>(Db.Command(sql, Db.Params(
            ("Id", staff.Id),
            ("FullName", staff.FullName),
            ("Email", staff.Email),
            ("Role", staff.Role.ToString()),
            ("IsActive", staff.IsActive),
            ("PasswordHash", string.IsNullOrWhiteSpace(staff.PasswordHash) ? null : staff.PasswordHash)), cancellationToken));
    }
}

public sealed class PatientRepository(PostgresConnectionFactory factory) : IPatientRepository
{
    public async Task<Patient> InsertAsync(Patient patient, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Patient_Insert", "Id", "FullName", "Age", "Sex", "Phone", "Address", "NextOfKinName", "NextOfKinPhone", "NextOfKinRelationship", "CreatedAt");
        return await connection.QuerySingleAsync<Patient>(Db.Command(sql, patient, cancellationToken));
    }

    public async Task<Patient?> GetByIdAsync(Guid patientId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Patient_GetById", "PatientId");
        return await connection.QuerySingleOrDefaultAsync<Patient>(Db.Command(sql, Db.Params(("PatientId", patientId)), cancellationToken));
    }

    public async Task<IReadOnlyList<Patient>> SearchAsync(string? name, string? phone, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Patient_Search", "Name", "Phone");
        var rows = await connection.QueryAsync<Patient>(Db.Command(sql, Db.Params(("Name", name), ("Phone", phone)), cancellationToken));
        return rows.ToArray();
    }
}

public sealed class EncounterRepository(PostgresConnectionFactory factory) : IEncounterRepository
{
    public async Task<Encounter> InsertAsync(Encounter encounter, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Encounter_Insert", "Id", "PatientId", "AdmissionType", "Status", "ArrivalMode", "ChiefComplaint", "RegisteredBy", "AdmittedAt", "DischargedAt", "CreatedAt", "UpdatedAt");
        return await connection.QuerySingleAsync<Encounter>(Db.Command(sql, ToParameters(encounter), cancellationToken));
    }

    public async Task<Encounter?> GetByIdAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Encounter_GetById", "EncounterId");
        return await connection.QuerySingleOrDefaultAsync<Encounter>(Db.Command(sql, Db.Params(("EncounterId", encounterId)), cancellationToken));
    }

    public async Task<IReadOnlyList<Encounter>> GetListAsync(EncounterStatus? status, DateOnly? date, AdmissionType? type, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Encounter_GetList", "Status", "Date", "AdmissionType");
        var parameters = Db.Params(("Status", Db.S(status)), ("Date", Db.D(date)), ("AdmissionType", Db.S(type)));
        var rows = await connection.QueryAsync<Encounter>(Db.Command(sql, parameters, cancellationToken));
        return rows.ToArray();
    }

    public async Task UpdateStatusAsync(Guid encounterId, EncounterStatus status, DateTimeOffset? dischargedAt, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Encounter_UpdateStatus", "EncounterId", "Status", "DischargedAt");
        var parameters = Db.Params(("EncounterId", encounterId), ("Status", status.ToString()), ("DischargedAt", dischargedAt));
        await connection.ExecuteAsync(Db.Command(sql, parameters, cancellationToken));
    }

    public async Task<EncounterDetail?> GetDetailAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT * FROM public.Encounters WHERE Id = @EncounterId;
            SELECT p.* FROM public.Patients p JOIN public.Encounters e ON e.PatientId = p.Id WHERE e.Id = @EncounterId;
            SELECT * FROM public.VitalSigns WHERE EncounterId = @EncounterId ORDER BY RecordedAt DESC;
            SELECT * FROM public.ConsultationNotes WHERE EncounterId = @EncounterId;
            SELECT * FROM public.Prescriptions WHERE EncounterId = @EncounterId;
            SELECT * FROM public.LabRequests WHERE EncounterId = @EncounterId;
            SELECT r.* FROM public.LabResults r JOIN public.LabRequests q ON q.Id = r.LabRequestId WHERE q.EncounterId = @EncounterId;
            SELECT * FROM public.DressingOrders WHERE EncounterId = @EncounterId;
            SELECT * FROM public.ContactTraces WHERE EncounterId = @EncounterId;
            """;

        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        using var multi = await connection.QueryMultipleAsync(Db.Command(sql, Db.Params(("EncounterId", encounterId)), cancellationToken));
        var encounter = await multi.ReadSingleOrDefaultAsync<Encounter>();
        if (encounter is null)
        {
            return null;
        }

        var patient = await multi.ReadSingleOrDefaultAsync<Patient>();
        var vitals = (await multi.ReadAsync<VitalSigns>()).ToArray();
        var consultation = await multi.ReadSingleOrDefaultAsync<ConsultationNote>();
        var prescriptions = (await multi.ReadAsync<Prescription>()).ToArray();
        var labRequests = (await multi.ReadAsync<LabRequest>()).ToArray();
        var labResults = (await multi.ReadAsync<LabResult>()).ToArray();
        var dressing = (await multi.ReadAsync<DressingOrder>()).ToArray();
        var contactTrace = await multi.ReadSingleOrDefaultAsync<ContactTrace>();
        return new EncounterDetail(encounter, patient, vitals, consultation, prescriptions, labRequests, labResults, dressing, contactTrace);
    }

    private static DynamicParameters ToParameters(Encounter encounter) => Db.Params(
        ("Id", encounter.Id),
        ("PatientId", encounter.PatientId),
        ("AdmissionType", encounter.AdmissionType.ToString()),
        ("Status", encounter.Status.ToString()),
        ("ArrivalMode", encounter.ArrivalMode.ToString()),
        ("ChiefComplaint", encounter.ChiefComplaint),
        ("RegisteredBy", encounter.RegisteredBy),
        ("AdmittedAt", encounter.AdmittedAt),
        ("DischargedAt", encounter.DischargedAt),
        ("CreatedAt", encounter.CreatedAt),
        ("UpdatedAt", encounter.UpdatedAt));
}

public sealed class VitalsRepository(PostgresConnectionFactory factory) : IVitalsRepository
{
    public async Task<VitalSigns> InsertAsync(VitalSigns vitalSigns, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_VitalSigns_Insert", "Id", "EncounterId", "RecordedBy", "BloodPressureSystolic", "BloodPressureDiastolic", "PulseRate", "Temperature", "Spo2", "RespiratoryRate", "Weight", "Notes", "RecordedAt");
        return await connection.QuerySingleAsync<VitalSigns>(Db.Command(sql, vitalSigns, cancellationToken));
    }

    public async Task<IReadOnlyList<VitalSigns>> GetByEncounterAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_VitalSigns_GetByEncounter", "EncounterId");
        var rows = await connection.QueryAsync<VitalSigns>(Db.Command(sql, Db.Params(("EncounterId", encounterId)), cancellationToken));
        return rows.ToArray();
    }

    public async Task<VitalSigns?> GetLatestAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_VitalSigns_GetLatest", "EncounterId");
        return await connection.QuerySingleOrDefaultAsync<VitalSigns>(Db.Command(sql, Db.Params(("EncounterId", encounterId)), cancellationToken));
    }
}

public sealed class ConsultationRepository(PostgresConnectionFactory factory) : IConsultationRepository
{
    public async Task<ConsultationNote?> GetByEncounterAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Consultation_GetByEncounter", "EncounterId");
        return await connection.QuerySingleOrDefaultAsync<ConsultationNote>(Db.Command(sql, Db.Params(("EncounterId", encounterId)), cancellationToken));
    }

    public async Task CreateWithChildrenAsync(ConsultationNote note, IReadOnlyList<Prescription> prescriptions, IReadOnlyList<LabRequest> labRequests, DressingOrder? dressingOrder, EncounterStatus nextStatus, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            var consultationSql = Db.Function("usp_Consultation_Insert", "Id", "EncounterId", "DoctorId", "Diagnosis", "ClinicalNotes", "RequiresLab", "RequiresDressing", "IsReferral", "ReferralFacility", "ReferralReason", "ConsultedAt");
            await connection.ExecuteAsync(Db.Command(consultationSql, note, cancellationToken, transaction));

            if (prescriptions.Count > 0)
            {
                const string prescriptionSql = "SELECT public.usp_Prescription_InsertBulk(CAST(@Prescriptions AS jsonb));";
                var parameters = Db.Params(("Prescriptions", PrescriptionJson(prescriptions)));
                await connection.ExecuteAsync(Db.Command(prescriptionSql, parameters, cancellationToken, transaction));
            }

            if (labRequests.Count > 0)
            {
                const string labRequestSql = "SELECT public.usp_LabRequest_InsertBulk(CAST(@LabRequests AS jsonb));";
                var parameters = Db.Params(("LabRequests", LabRequestJson(labRequests)));
                await connection.ExecuteAsync(Db.Command(labRequestSql, parameters, cancellationToken, transaction));
            }

            if (dressingOrder is not null)
            {
                var dressingSql = Db.Function("usp_DressingOrder_Insert", "Id", "ConsultationNoteId", "EncounterId", "Instructions", "Status", "PerformedBy", "ProcedureNotes", "CompletedAt", "CreatedAt");
                var parameters = DressingParameters(dressingOrder);
                await connection.ExecuteAsync(Db.Command(dressingSql, parameters, cancellationToken, transaction));
            }

            var encounterSql = Db.Function("usp_Encounter_UpdateStatus", "EncounterId", "Status", "DischargedAt");
            var encounterParameters = Db.Params(
                ("EncounterId", note.EncounterId),
                ("Status", nextStatus.ToString()),
                ("DischargedAt", nextStatus == EncounterStatus.Referred ? DateTimeOffset.UtcNow : null));
            await connection.ExecuteAsync(Db.Command(encounterSql, encounterParameters, cancellationToken, transaction));
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static string PrescriptionJson(IEnumerable<Prescription> rows) =>
        JsonSerializer.Serialize(rows.Select(row => new
        {
            row.Id,
            row.ConsultationNoteId,
            row.EncounterId,
            row.DrugName,
            row.Dosage,
            row.Frequency,
            row.Duration,
            Route = row.Route.ToString(),
            row.Instructions,
            Status = row.Status.ToString(),
            IssuedAt = row.IssuedAt.ToUniversalTime()
        }));

    private static string LabRequestJson(IEnumerable<LabRequest> rows) =>
        JsonSerializer.Serialize(rows.Select(row => new
        {
            row.Id,
            row.ConsultationNoteId,
            row.EncounterId,
            row.TestName,
            row.ClinicalIndication,
            Status = row.Status.ToString(),
            RequestedAt = row.RequestedAt.ToUniversalTime()
        }));

    private static DynamicParameters DressingParameters(DressingOrder order) => Db.Params(
        ("Id", order.Id),
        ("ConsultationNoteId", order.ConsultationNoteId),
        ("EncounterId", order.EncounterId),
        ("Instructions", order.Instructions),
        ("Status", order.Status.ToString()),
        ("PerformedBy", order.PerformedBy),
        ("ProcedureNotes", order.ProcedureNotes),
        ("CompletedAt", order.CompletedAt),
        ("CreatedAt", order.CreatedAt));
}

public sealed class PrescriptionRepository(PostgresConnectionFactory factory) : IPrescriptionRepository
{
    public async Task<Prescription?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Prescription_GetById", "Id");
        return await connection.QuerySingleOrDefaultAsync<Prescription>(Db.Command(sql, Db.Params(("Id", id)), cancellationToken));
    }

    public async Task<IReadOnlyList<Prescription>> GetWorklistAsync(PrescriptionStatus? status, DateOnly? date, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Prescription_GetWorklist", "Status", "Date");
        var rows = await connection.QueryAsync<Prescription>(Db.Command(sql, Db.Params(("Status", Db.S(status)), ("Date", Db.D(date))), cancellationToken));
        return rows.ToArray();
    }

    public async Task UpdateStatusAsync(Guid id, PrescriptionStatus status, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Prescription_UpdateStatus", "Id", "Status");
        await connection.ExecuteAsync(Db.Command(sql, Db.Params(("Id", id), ("Status", status.ToString())), cancellationToken));
    }

    public async Task<bool> AllHandedOverForEncounterAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Prescription_AllHandedOverForEncounter", "EncounterId");
        return await connection.QuerySingleAsync<bool>(Db.Command(sql, Db.Params(("EncounterId", encounterId)), cancellationToken));
    }
}

public sealed class DispensingRepository(PostgresConnectionFactory factory) : IDispensingRepository
{
    public async Task<Dispensing> InsertAsync(Dispensing dispensing, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Dispensing_Insert", "Id", "PrescriptionId", "PharmacistId", "DrugName", "QuantityDispensed", "BatchNumber", "ExpiryDate", "Notes", "DispensedAt");
        return await connection.QuerySingleAsync<Dispensing>(Db.Command(sql, dispensing, cancellationToken));
    }

    public async Task<Dispensing?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Dispensing_GetById", "Id");
        return await connection.QuerySingleOrDefaultAsync<Dispensing>(Db.Command(sql, Db.Params(("Id", id)), cancellationToken));
    }
}

public sealed class DrugHandoverRepository(PostgresConnectionFactory factory) : IDrugHandoverRepository
{
    public async Task<DrugHandover> InsertAsync(DrugHandover handover, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_DrugHandover_Insert", "Id", "DispensingId", "EncounterId", "ProtocolOfficerId", "PatientNameVerified", "DrugListVerified", "DosageCounsellingDone", "DurationCounsellingDone", "CounsellingNotes", "HandoverAt");
        return await connection.QuerySingleAsync<DrugHandover>(Db.Command(sql, handover, cancellationToken));
    }

    public async Task<IReadOnlyList<DrugHandover>> GetWorklistAsync(string? status, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_DrugHandover_GetWorklist", "Status");
        var rows = await connection.QueryAsync<DrugHandover>(Db.Command(sql, Db.Params(("Status", status)), cancellationToken));
        return rows.ToArray();
    }

    public async Task<DrugHandover?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_DrugHandover_GetById", "Id");
        return await connection.QuerySingleOrDefaultAsync<DrugHandover>(Db.Command(sql, Db.Params(("Id", id)), cancellationToken));
    }

    public async Task ConfirmAsync(DrugHandover handover, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_DrugHandover_Confirm", "Id", "DispensingId", "EncounterId", "ProtocolOfficerId", "PatientNameVerified", "DrugListVerified", "DosageCounsellingDone", "DurationCounsellingDone", "CounsellingNotes", "HandoverAt");
        await connection.ExecuteAsync(Db.Command(sql, handover, cancellationToken));
    }
}

public sealed class LabRepository(PostgresConnectionFactory factory) : ILabRepository
{
    public async Task<IReadOnlyList<LabRequest>> GetRequestsAsync(LabRequestStatus? status, DateOnly? date, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_LabRequest_GetWorklist", "Status", "Date");
        var rows = await connection.QueryAsync<LabRequest>(Db.Command(sql, Db.Params(("Status", Db.S(status)), ("Date", Db.D(date))), cancellationToken));
        return rows.ToArray();
    }

    public async Task<LabRequest?> GetRequestAsync(Guid requestId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_LabRequest_GetById", "RequestId");
        return await connection.QuerySingleOrDefaultAsync<LabRequest>(Db.Command(sql, Db.Params(("RequestId", requestId)), cancellationToken));
    }

    public async Task<LabResult> InsertResultAsync(LabResult result, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_LabResult_Insert", "Id", "LabRequestId", "ScientistId", "TestName", "Findings", "Conclusion", "Values", "CompletedAt");
        return await connection.QuerySingleAsync<LabResult>(Db.Command(sql, result, cancellationToken));
    }

    public async Task<IReadOnlyList<LabResult>> GetResultsByEncounterAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_LabResult_GetByEncounter", "EncounterId");
        var rows = await connection.QueryAsync<LabResult>(Db.Command(sql, Db.Params(("EncounterId", encounterId)), cancellationToken));
        return rows.ToArray();
    }
}

public sealed class DressingRepository(PostgresConnectionFactory factory) : IDressingRepository
{
    public async Task<IReadOnlyList<DressingOrder>> GetWorklistAsync(DressingOrderStatus? status, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_DressingOrder_GetWorklist", "Status");
        var rows = await connection.QueryAsync<DressingOrder>(Db.Command(sql, Db.Params(("Status", Db.S(status))), cancellationToken));
        return rows.ToArray();
    }

    public async Task<DressingOrder?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_DressingOrder_GetById", "OrderId");
        return await connection.QuerySingleOrDefaultAsync<DressingOrder>(Db.Command(sql, Db.Params(("OrderId", orderId)), cancellationToken));
    }

    public async Task CompleteAsync(Guid orderId, Guid performedBy, string? procedureNotes, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_DressingOrder_Complete", "OrderId", "PerformedBy", "ProcedureNotes");
        var parameters = Db.Params(("OrderId", orderId), ("PerformedBy", performedBy), ("ProcedureNotes", procedureNotes));
        await connection.ExecuteAsync(Db.Command(sql, parameters, cancellationToken));
    }
}

public sealed class ContactTraceRepository(PostgresConnectionFactory factory) : IContactTraceRepository
{
    public async Task<ContactTrace> InsertAsync(ContactTrace contactTrace, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_ContactTrace_Insert", "Id", "EncounterId", "RecordedBy", "NextOfKinName", "NextOfKinPhone", "NextOfKinRelationship", "ResidentialAddress", "WorkplaceAddress", "DischargeNotes", "ReferralDestination", "RecordedAt");
        return await connection.QuerySingleAsync<ContactTrace>(Db.Command(sql, contactTrace, cancellationToken));
    }

    public async Task<ContactTrace?> GetByEncounterAsync(Guid encounterId, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_ContactTrace_GetByEncounter", "EncounterId");
        return await connection.QuerySingleOrDefaultAsync<ContactTrace>(Db.Command(sql, Db.Params(("EncounterId", encounterId)), cancellationToken));
    }

    public async Task<ContactTrace> UpdateAsync(ContactTrace contactTrace, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_ContactTrace_Update", "Id", "EncounterId", "RecordedBy", "NextOfKinName", "NextOfKinPhone", "NextOfKinRelationship", "ResidentialAddress", "WorkplaceAddress", "DischargeNotes", "ReferralDestination", "RecordedAt");
        return await connection.QuerySingleAsync<ContactTrace>(Db.Command(sql, contactTrace, cancellationToken));
    }
}

public sealed class RegisterRepository(PostgresConnectionFactory factory) : IRegisterRepository
{
    public async Task<IReadOnlyList<DrugRegisterEntry>> GetDrugsAsync(DateOnly? date, int page, int limit, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Register_GetDrugs", "Date", "Page", "Limit");
        var rows = await connection.QueryAsync<DrugRegisterEntry>(Db.Command(sql, Db.Params(("Date", Db.D(date)), ("Page", page), ("Limit", limit)), cancellationToken));
        return rows.ToArray();
    }

    public async Task<IReadOnlyList<DrugRegisterEntry>> ExportDrugsAsync(DateOnly? date, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_Register_ExportDrugs", "Date");
        var rows = await connection.QueryAsync<DrugRegisterEntry>(Db.Command(sql, Db.Params(("Date", Db.D(date))), cancellationToken));
        return rows.ToArray();
    }
}

public sealed class ServiceWindowRepository(PostgresConnectionFactory factory) : IServiceWindowRepository
{
    public async Task<ServiceTimeWindow> InsertAsync(ServiceTimeWindow window, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_ServiceWindow_Insert", "Id", "Date", "ColdCaseOpenTime", "ColdCaseCloseTime", "CreatedBy", "CreatedAt");
        var parameters = Db.Params(("Id", window.Id), ("Date", window.Date), ("ColdCaseOpenTime", window.ColdCaseOpenTime), ("ColdCaseCloseTime", window.ColdCaseCloseTime), ("CreatedBy", window.CreatedBy), ("CreatedAt", window.CreatedAt));
        return await connection.QuerySingleAsync<ServiceTimeWindow>(Db.Command(sql, parameters, cancellationToken));
    }

    public async Task<ServiceTimeWindow?> GetCurrentAsync(DateOnly date, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_ServiceWindow_GetCurrent", "Date");
        return await connection.QuerySingleOrDefaultAsync<ServiceTimeWindow>(Db.Command(sql, Db.Params(("Date", date)), cancellationToken));
    }

    public async Task<ServiceTimeWindow> UpdateAsync(Guid windowId, TimeOnly openTime, TimeOnly closeTime, CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateOpenConnectionAsync(cancellationToken);
        var sql = Db.Function("usp_ServiceWindow_Update", "WindowId", "ColdCaseOpenTime", "ColdCaseCloseTime");
        var parameters = Db.Params(("WindowId", windowId), ("ColdCaseOpenTime", openTime), ("ColdCaseCloseTime", closeTime));
        return await connection.QuerySingleAsync<ServiceTimeWindow>(Db.Command(sql, parameters, cancellationToken));
    }
}
