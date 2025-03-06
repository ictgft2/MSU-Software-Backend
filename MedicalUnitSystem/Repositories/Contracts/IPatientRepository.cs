using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface IPatientRepository : IRepository<Patient>
    {
        bool PatientNumberExist(string patientNumber);
        Task<bool> PatientExistsAsync(int patientId);
        Task<bool> PatientExistsAsync(string patientPhoneNumber);
        Task<Patient> GetPatientByPhoneAsync(string patientPhoneNumber);
    }
}
