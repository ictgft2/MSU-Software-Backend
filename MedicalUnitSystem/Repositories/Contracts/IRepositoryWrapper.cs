namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface IRepositoryWrapper
    {
        IConsultationRepository Consultation { get; }
        IPatientRepository Patient { get; }
        ILaboratoryTestRepository LaboratoryTest { get; }
        ILaboratoryTestTypeRepository LaboratoryTestType { get; }
        void Save();
    }
}
