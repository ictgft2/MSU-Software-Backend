using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface ILaboratoryTestRepository : IRepository<LaboratoryTest>
    {
        Task<bool> LaboratoryTestExistsAsync(int laboratoryTestId);
    }
}
