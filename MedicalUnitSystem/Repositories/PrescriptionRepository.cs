using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MedicalUnitSystem.Repositories
{
    public class PrescriptionRepository : Repository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(HospitalContext context) : base(context)
        {
        }

        public async Task<bool> PrescriptionExistsAsync(int prescriptionId)
        {
            if (prescriptionId is 0 || prescriptionId is int.MinValue)
            {
                throw new ArgumentNullException(nameof(prescriptionId));
            }

            return await Context.Prescriptions.AnyAsync(d => d.PrescriptionId == prescriptionId);
        }
    }
}
