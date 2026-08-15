using Gilead.Application.DTOs;
using Gilead.Domain.Entities;
using Gilead.Domain.Enums;

namespace Gilead.Application.Interfaces;

public interface IPatientRepository
{
    Task<Patient> InsertAsync(Patient patient, CancellationToken cancellationToken);
    Task<Patient?> GetByIdAsync(Guid patientId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Patient>> SearchAsync(string? name, string? phone, CancellationToken cancellationToken);
}

public interface IEncounterRepository
{
    Task<Encounter> InsertAsync(Encounter encounter, CancellationToken cancellationToken);
    Task<Encounter?> GetByIdAsync(Guid encounterId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Encounter>> GetListAsync(EncounterStatus? status, DateOnly? date, AdmissionType? type, CancellationToken cancellationToken);
    Task UpdateStatusAsync(Guid encounterId, EncounterStatus status, DateTimeOffset? dischargedAt, CancellationToken cancellationToken);
    Task<EncounterDetail?> GetDetailAsync(Guid encounterId, CancellationToken cancellationToken);
}

public interface IVitalsRepository
{
    Task<VitalSigns> InsertAsync(VitalSigns vitalSigns, CancellationToken cancellationToken);
    Task<IReadOnlyList<VitalSigns>> GetByEncounterAsync(Guid encounterId, CancellationToken cancellationToken);
    Task<VitalSigns?> GetLatestAsync(Guid encounterId, CancellationToken cancellationToken);
}

public interface IConsultationRepository
{
    Task<ConsultationNote?> GetByEncounterAsync(Guid encounterId, CancellationToken cancellationToken);
    Task CreateWithChildrenAsync(ConsultationNote note, IReadOnlyList<Prescription> prescriptions, IReadOnlyList<LabRequest> labRequests, DressingOrder? dressingOrder, EncounterStatus nextStatus, CancellationToken cancellationToken);
}

public interface IPrescriptionRepository
{
    Task<Prescription?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Prescription>> GetWorklistAsync(PrescriptionStatus? status, DateOnly? date, CancellationToken cancellationToken);
    Task UpdateStatusAsync(Guid id, PrescriptionStatus status, CancellationToken cancellationToken);
    Task<bool> AllHandedOverForEncounterAsync(Guid encounterId, CancellationToken cancellationToken);
}

public interface IDispensingRepository
{
    Task<Dispensing> InsertAsync(Dispensing dispensing, CancellationToken cancellationToken);
    Task<Dispensing?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}

public interface IDrugHandoverRepository
{
    Task<DrugHandover> InsertAsync(DrugHandover handover, CancellationToken cancellationToken);
    Task<IReadOnlyList<DrugHandover>> GetWorklistAsync(string? status, CancellationToken cancellationToken);
    Task<DrugHandover?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task ConfirmAsync(DrugHandover handover, CancellationToken cancellationToken);
}

public interface ILabRepository
{
    Task<IReadOnlyList<LabRequest>> GetRequestsAsync(LabRequestStatus? status, DateOnly? date, CancellationToken cancellationToken);
    Task<LabRequest?> GetRequestAsync(Guid requestId, CancellationToken cancellationToken);
    Task<LabResult> InsertResultAsync(LabResult result, CancellationToken cancellationToken);
    Task<IReadOnlyList<LabResult>> GetResultsByEncounterAsync(Guid encounterId, CancellationToken cancellationToken);
}

public interface IDressingRepository
{
    Task<IReadOnlyList<DressingOrder>> GetWorklistAsync(DressingOrderStatus? status, CancellationToken cancellationToken);
    Task<DressingOrder?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);
    Task CompleteAsync(Guid orderId, Guid performedBy, string? procedureNotes, CancellationToken cancellationToken);
}

public interface IContactTraceRepository
{
    Task<ContactTrace> InsertAsync(ContactTrace contactTrace, CancellationToken cancellationToken);
    Task<ContactTrace?> GetByEncounterAsync(Guid encounterId, CancellationToken cancellationToken);
    Task<ContactTrace> UpdateAsync(ContactTrace contactTrace, CancellationToken cancellationToken);
}

public interface IRegisterRepository
{
    Task<IReadOnlyList<DrugRegisterEntry>> GetDrugsAsync(DateOnly? date, int page, int limit, CancellationToken cancellationToken);
    Task<IReadOnlyList<DrugRegisterEntry>> ExportDrugsAsync(DateOnly? date, CancellationToken cancellationToken);
}

public interface IServiceWindowRepository
{
    Task<ServiceTimeWindow> InsertAsync(ServiceTimeWindow window, CancellationToken cancellationToken);
    Task<ServiceTimeWindow?> GetCurrentAsync(DateOnly date, CancellationToken cancellationToken);
    Task<ServiceTimeWindow> UpdateAsync(Guid windowId, TimeOnly openTime, TimeOnly closeTime, CancellationToken cancellationToken);
}

public interface IQueueCacheService
{
    Task JoinAsync(Guid encounterId, DateOnly date, CancellationToken cancellationToken);
    Task DequeueAsync(Guid encounterId, DateOnly date, CancellationToken cancellationToken);
    Task<long?> GetPositionAsync(Guid encounterId, DateOnly date, CancellationToken cancellationToken);
    Task<IReadOnlyList<QueueEntry>> GetFullListAsync(DateOnly date, CancellationToken cancellationToken);
}
