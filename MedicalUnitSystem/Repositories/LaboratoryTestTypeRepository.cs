using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MedicalUnitSystem.Repositories
{
    public class LaboratoryTestTypeRepository : Repository<LaboratoryTestType>, ILaboratoryTestTypeRepository
    {
        public LaboratoryTestTypeRepository(HospitalContext context) : base(context)
        {
        }

        public async Task<bool> LaboratoryTestTypeExistsAsync(int laboratoryTestTypeId)
        {
            if (laboratoryTestTypeId is 0 || laboratoryTestTypeId is int.MinValue)
            {
                throw new ArgumentNullException(nameof(laboratoryTestTypeId));
            }
            return await Context.LaboratoryTestTypes.AnyAsync(l => l.LaboratoryTestTypeId == laboratoryTestTypeId);
        }
    }
}
