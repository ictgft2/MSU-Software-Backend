namespace MedicalUnitSystem.Services.Contracts
{
    public interface IServiceWrapper
    {
        IAdmissionService Admission { get; }
        IPatientService Patient { get; }
        IConsultationService Consultation { get; }
        ILaboratoryTestService LaboratoryTest { get; }
        ILaboratoryTestTypeService LaboratoryTestType { get; }
        IVitalsService Vitals { get; }
        IWaitingQueueService WaitingQueue { get; }
        IPrescriptionService Prescription { get; }
        IDoctorService Doctor { get; }
        IGenderService Gender { get; }
    }
}
