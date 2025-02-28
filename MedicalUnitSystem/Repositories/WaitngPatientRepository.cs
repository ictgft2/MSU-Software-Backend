using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;

namespace MedicalUnitSystem.Repositories
{
    public class WaitngPatientRepository : Repository<WaitingPatient>, IWaitingPatientRepository
    {
        public WaitngPatientRepository(HospitalContext context) : base(context)
        {
        }
    }
}
