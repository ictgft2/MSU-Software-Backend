using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;

namespace MedicalUnitSystem.Repositories
{
    public class VitalsRepository : Repository<Vital>, IVitalsRepository
    {
        public VitalsRepository(HospitalContext context) : base(context)
        {
        }
    }
}
