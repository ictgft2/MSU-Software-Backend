using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MedicalUnitSystem.Repositories
{
    public class VitalsRepository : Repository<Vital>, IVitalsRepository
    {
        public VitalsRepository(HospitalContext context) : base(context)
        {
        }
        public async Task<bool> VitalsExistsAsync(int vitalsId)
        {
            if (vitalsId is 0 || vitalsId is int.MinValue)
            {
                throw new ArgumentNullException(nameof(vitalsId));
            }

            return await Context.Vitals.AnyAsync(d => d.VitalsId == vitalsId);
        }
    }
}
