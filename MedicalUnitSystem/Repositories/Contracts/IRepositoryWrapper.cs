namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface IRepositoryWrapper
    {
        IConsultationRepository Consultations { get; }
        IPatientRepository Patients { get; }
        ILaboratoryTestRepository LaboratoryTests { get; }
        ILaboratoryTestTypeRepository LaboratoryTestTypes { get; }
        IVitalsRepository Vitals { get; }
        IWaitingPatientRepository WaitingPatients { get; }
        IPrescriptionRepository Prescriptions { get; }
        IDoctorRepository Doctors { get; }
        IGenderRepository Genders { get; }
        IAdmissionRepository Admissions { get; }
        void Save();
    }
}
