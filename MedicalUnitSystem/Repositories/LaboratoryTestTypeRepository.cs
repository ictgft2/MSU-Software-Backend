using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;

namespace MedicalUnitSystem.Repositories
{
    public class LaboratoryTestTypeRepository : Repository<LaboratoryTestType>, ILaboratoryTestTypeRepository
    {
        public LaboratoryTestTypeRepository(HospitalContext context) : base(context)
        {
        }
    }
}
