using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;

namespace MedicalUnitSystem.Repositories
{
    public class LaboratoryTestRepository : Repository<LaboratoryTest>, ILaboratoryTestRepository
    {
        public LaboratoryTestRepository(HospitalContext context) : base(context)
        {
        }
    }
}
