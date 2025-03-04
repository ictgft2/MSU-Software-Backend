using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<bool> DoctorExistsAsync(int doctorId);
    }
}
