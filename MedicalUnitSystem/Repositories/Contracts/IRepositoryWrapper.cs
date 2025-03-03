namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface IRepositoryWrapper
    {
        IConsultationRepository Consultation { get; }
        IPatientRepository Patient { get; }
        ILaboratoryTestRepository LaboratoryTest { get; }
        ILaboratoryTestTypeRepository LaboratoryTestType { get; }
        IVitalsRepository Vitals { get; }
        IWaitingPatientRepository WaitingPatient { get; }
        IPrescriptionRepository Prescription { get; }
        IDoctorRepository Doctor { get; }
        IGenderRepository Gender { get; }
        void Save();
    }
}
