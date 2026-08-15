using Gilead.Domain.Entities;
using Gilead.Domain.Enums;

namespace Gilead.Application.DTOs;

public sealed record RegisterPatientRequest(string FullName, int Age, string Sex, string Phone, string Address, string NextOfKinName, string NextOfKinPhone, string NextOfKinRelationship);
public sealed record OpenEncounterRequest(Guid PatientId, AdmissionType AdmissionType, ArrivalMode ArrivalMode, string ChiefComplaint, Guid RegisteredBy);
public sealed record AdvanceEncounterStatusRequest(EncounterStatus Status);
public sealed record RecordVitalsRequest(Guid RecordedBy, int? BloodPressureSystolic, int? BloodPressureDiastolic, int? PulseRate, decimal? Temperature, int? Spo2, int? RespiratoryRate, decimal? Weight, string? Notes);
public sealed record PrescriptionPlanRequest(string DrugName, string Dosage, string Frequency, string Duration, DrugRoute Route, string? Instructions);
public sealed record LabTestPlanRequest(string TestName, string ClinicalIndication);
public sealed record TreatmentPlanRequest(IReadOnlyList<PrescriptionPlanRequest> Prescriptions, IReadOnlyList<LabTestPlanRequest> LabTests, bool RequiresDressing, string? DressingInstructions, bool IsReferral, string? ReferralFacility, string? ReferralReason);
public sealed record SubmitConsultationRequest(Guid DoctorId, IReadOnlyList<string> Diagnosis, string ClinicalNotes, TreatmentPlanRequest TreatmentPlan);
public sealed record DispensePrescriptionRequest(Guid PharmacistId, int QuantityDispensed, string BatchNumber, DateOnly ExpiryDate, string? Notes);
public sealed record ConfirmHandoverRequest(Guid ProtocolOfficerId, bool PatientNameVerified, bool DrugListVerified, bool DosageCounsellingDone, bool DurationCounsellingDone, string? CounsellingNotes);
public sealed record CompleteDressingOrderRequest(Guid PerformedBy, string? ProcedureNotes);
public sealed record LabResultValueRequest(string Parameter, string Value, string Unit, string ReferenceRange);
public sealed record PostLabResultRequest(Guid ScientistId, string TestName, string Findings, IReadOnlyList<LabResultValueRequest> Values, string Conclusion);
public sealed record ContactTraceRequest(Guid RecordedBy, string NextOfKinName, string NextOfKinPhone, string NextOfKinRelationship, string ResidentialAddress, string WorkplaceAddress, string DischargeNotes, string? ReferralDestination);
public sealed record SetServiceWindowRequest(DateOnly Date, TimeOnly ColdCaseOpenTime, TimeOnly ColdCaseCloseTime, Guid CreatedBy);
public sealed record UpdateServiceWindowRequest(TimeOnly ColdCaseOpenTime, TimeOnly ColdCaseCloseTime);
public sealed record QueueEntry(Guid EncounterId, double Score, long Position);
public sealed record ServiceWindowStatus(bool IsOpen, ServiceTimeWindow? Window);
public sealed record EncounterDetail(Encounter Encounter, Patient? Patient, IReadOnlyList<VitalSigns> Vitals, ConsultationNote? Consultation, IReadOnlyList<Prescription> Prescriptions, IReadOnlyList<LabRequest> LabRequests, IReadOnlyList<LabResult> LabResults, IReadOnlyList<DressingOrder> DressingOrders, ContactTrace? ContactTrace);
public sealed record DrugRegisterEntry(Guid HandoverId, Guid EncounterId, Guid PrescriptionId, string DrugName, string Dosage, string Frequency, string Duration, string BatchNumber, int QuantityDispensed, DateOnly ExpiryDate, DateTimeOffset HandoverAt);
