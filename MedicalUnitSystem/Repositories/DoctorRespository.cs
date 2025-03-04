using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MedicalUnitSystem.Repositories
{
    public class DoctorRespository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRespository(HospitalContext context) : base(context)
        {
        }
        public async Task<bool> DoctorExistsAsync(int doctorId)
        {
            if(doctorId is 0 || doctorId is int.MinValue)
            {
                throw new ArgumentNullException(nameof(doctorId));
            }

            return await Context.Doctors.AnyAsync(d => d.DoctorId == doctorId);
        }
    }
}
