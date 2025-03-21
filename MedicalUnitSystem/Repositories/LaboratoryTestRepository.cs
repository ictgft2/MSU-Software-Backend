using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MedicalUnitSystem.Repositories
{
    public class LaboratoryTestRepository : Repository<LaboratoryTest>, ILaboratoryTestRepository
    {
        public LaboratoryTestRepository(HospitalContext context) : base(context)
        {
        }

        public async Task<bool> LaboratoryTestExistsAsync(int laboratoryTestId)
        {
            if (laboratoryTestId is 0 || laboratoryTestId is int.MinValue)
            {
                throw new ArgumentNullException(nameof(laboratoryTestId));
            }

            return await Context.LaboratoryTests.AnyAsync(d => d.LaboratoryTestId == laboratoryTestId);
        }
    }
}
