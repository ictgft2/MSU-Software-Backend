using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;

namespace MedicalUnitSystem.Repositories
{
    public class AdmissionRepository : Repository<Admission>, IAdmissionRepository
    {
        public AdmissionRepository(HospitalContext context) : base(context)
        {
        }
    }
}
