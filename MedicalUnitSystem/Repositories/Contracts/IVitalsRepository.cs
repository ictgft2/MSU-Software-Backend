using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface IVitalsRepository : IRepository<Vital>
    {
        Task<bool> VitalsExistsAsync(int vitalsId);
    }
}
