using Gilead.Application.DTOs;
using Gilead.Domain.Entities;
using Gilead.Domain.Enums;

namespace Gilead.Application.Interfaces;

public interface IPatientService
{
    Task<ServiceResult<Patient>> RegisterAsync(RegisterPatientRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<Patient>> GetByIdAsync(Guid patientId, CancellationToken cancellationToken);
    Task<ServiceResult<IReadOnlyList<Patient>>> SearchAsync(string? name, string? phone, CancellationToken cancellationToken);
}

public interface IEncounterService
{
    Task<ServiceResult<Encounter>> OpenAsync(OpenEncounterRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<EncounterDetail>> GetDetailAsync(Guid encounterId, CancellationToken cancellationToken);
    Task<ServiceResult<IReadOnlyList<Encounter>>> ListAsync(EncounterStatus? status, DateOnly? date, AdmissionType? type, CancellationToken cancellationToken);
    Task<ServiceResult> AdvanceStatusAsync(Guid encounterId, EncounterStatus status, CancellationToken cancellationToken);
}

public interface IQueueService
{
    Task<ServiceResult> JoinAsync(Guid encounterId, CancellationToken cancellationToken);
    Task<ServiceResult> DequeueAsync(Guid encounterId, CancellationToken cancellationToken);
    Task<ServiceResult<long>> GetPositionAsync(Guid encounterId, CancellationToken cancellationToken);
    Task<ServiceResult<IReadOnlyList<QueueEntry>>> GetFullListAsync(CancellationToken cancellationToken);
}

public interface IVitalsService
{
    Task<ServiceResult<VitalSigns>> RecordAsync(Guid encounterId, RecordVitalsRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<IReadOnlyList<VitalSigns>>> GetByEncounterAsync(Guid encounterId, CancellationToken cancellationToken);
    Task<ServiceResult<VitalSigns>> GetLatestAsync(Guid encounterId, CancellationToken cancellationToken);
}

public interface IConsultationService
{
    Task<ServiceResult<ConsultationNote>> SubmitAsync(Guid encounterId, SubmitConsultationRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<ConsultationNote>> GetByEncounterAsync(Guid encounterId, CancellationToken cancellationToken);
}

public interface ILabService
{
    Task<ServiceResult<IReadOnlyList<LabRequest>>> GetRequestsAsync(LabRequestStatus? status, DateOnly? date, CancellationToken cancellationToken);
    Task<ServiceResult<LabRequest>> GetRequestAsync(Guid requestId, CancellationToken cancellationToken);
    Task<ServiceResult<LabResult>> PostResultAsync(Guid requestId, PostLabResultRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<IReadOnlyList<LabResult>>> GetResultsByEncounterAsync(Guid encounterId, CancellationToken cancellationToken);
}

public interface IDressingService
{
    Task<ServiceResult<IReadOnlyList<DressingOrder>>> GetWorklistAsync(DressingOrderStatus? status, CancellationToken cancellationToken);
    Task<ServiceResult<DressingOrder>> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);
    Task<ServiceResult> CompleteAsync(Guid orderId, CompleteDressingOrderRequest request, CancellationToken cancellationToken);
}

public interface IPharmacyService
{
    Task<ServiceResult<IReadOnlyList<Prescription>>> GetWorklistAsync(PrescriptionStatus? status, DateOnly? date, CancellationToken cancellationToken);
    Task<ServiceResult<Prescription>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ServiceResult<Dispensing>> DispenseAsync(Guid id, DispensePrescriptionRequest request, CancellationToken cancellationToken);
}

public interface IProtocolService
{
    Task<ServiceResult<IReadOnlyList<DrugHandover>>> GetWorklistAsync(string? status, CancellationToken cancellationToken);
    Task<ServiceResult<DrugHandover>> GetByIdAsync(Guid handoverId, CancellationToken cancellationToken);
    Task<ServiceResult> ConfirmAsync(Guid handoverId, ConfirmHandoverRequest request, CancellationToken cancellationToken);
}

public interface IContactTraceService
{
    Task<ServiceResult<ContactTrace>> RecordAsync(Guid encounterId, ContactTraceRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<ContactTrace>> GetAsync(Guid encounterId, CancellationToken cancellationToken);
    Task<ServiceResult<ContactTrace>> UpdateAsync(Guid encounterId, ContactTraceRequest request, CancellationToken cancellationToken);
}

public interface IRegisterService
{
    Task<ServiceResult<IReadOnlyList<DrugRegisterEntry>>> GetDrugsAsync(DateOnly? date, int page, int limit, CancellationToken cancellationToken);
    Task<ServiceResult<string>> ExportDrugsAsync(DateOnly? date, string? format, CancellationToken cancellationToken);
}

public interface IServiceWindowService
{
    Task<ServiceResult<ServiceWindowStatus>> GetCurrentAsync(CancellationToken cancellationToken);
    Task<ServiceResult<ServiceTimeWindow>> SetTodayAsync(SetServiceWindowRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<ServiceTimeWindow>> UpdateAsync(Guid windowId, UpdateServiceWindowRequest request, CancellationToken cancellationToken);
}
