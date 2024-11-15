namespace MedicalUnitSystem.Services.Contracts
{
    public interface IServiceWrapper
    {
        IPatientService Patient { get; }
    }
}
