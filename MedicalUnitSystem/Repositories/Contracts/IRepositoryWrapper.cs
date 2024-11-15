namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface IRepositoryWrapper
    {
        IConsultationRepository Consultation { get; }
        IPatientRepository Patient { get; }
        void Save();
    }
}
