using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MedicalUnitSystem.Repositories
{
    public class GenderRepository : Repository<Gender>, IGenderRepository
    {
        public GenderRepository(HospitalContext context) : base(context)
        {
        }

        public async Task<bool> GenderExistsAsync(int genderId)
        {
            if (genderId is 0 || genderId is int.MinValue)
            {
                throw new ArgumentNullException(nameof(genderId));
            }

            return await Context.Genders.AnyAsync(d => d.GenderId == genderId);
        }
    }
}
