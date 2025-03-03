using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MedicalUnitSystem.Repositories
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(HospitalContext context) : base(context)
        {
        }

        public bool PatientNumberExist(string patientNumber)
        {
            return this.Context.Patients.Any(p => p.PatientNumber == patientNumber);
        }
    }
}
