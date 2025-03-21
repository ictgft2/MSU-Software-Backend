using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface IGenderRepository : IRepository<Gender>
    {
        Task<bool> GenderExistsAsync(int genderId);
    }
}
