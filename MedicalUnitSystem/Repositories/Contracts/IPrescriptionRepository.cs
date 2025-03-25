using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface IPrescriptionRepository : IRepository<Prescription>
    {
        Task<bool> PrescriptionExistsAsync(int prescriptionId);
    }
}
