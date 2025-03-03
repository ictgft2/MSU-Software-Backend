namespace MedicalUnitSystem.Services.Contracts
{
    public interface IServiceWrapper
    {
        IPatientService Patient { get; }
        IConsultationService Consultation { get; }
        ILaboratoryTestService LaboratoryTest { get; }
        ILaboratoryTestTypeService LaboratoryTestType { get; }
        IVitalsService Vitals { get; }
        IWaitingPatientService WaitingPatient { get; }
        IDoctorService Doctor { get; }
        IGenderService Gender { get; }
    }
}
