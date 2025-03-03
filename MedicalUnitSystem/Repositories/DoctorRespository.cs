using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;

namespace MedicalUnitSystem.Repositories
{
    public class DoctorRespository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRespository(HospitalContext context) : base(context)
        {
        }
    }
}
