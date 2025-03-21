using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface ILaboratoryTestTypeRepository : IRepository<LaboratoryTestType>
    {
        Task<bool> LaboratoryTestTypeExistsAsync(int patientId);
    }
}
