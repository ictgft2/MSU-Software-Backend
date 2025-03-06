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

        public async Task<bool> PatientExistsAsync(int patientId)
        {
            if (patientId is 0 || patientId is int.MinValue)
            {
                throw new ArgumentNullException(nameof(patientId));
            }

            return await Context.Patients.AnyAsync(d => d.PatientId == patientId);
        }
        public async Task<bool> PatientExistsAsync(string patientPhoneNumber)
        {
            if (string.IsNullOrEmpty(patientPhoneNumber))
            {
                throw new ArgumentNullException(nameof(patientPhoneNumber));
            }

            return await Context.Patients.AnyAsync(d => d.Phone == patientPhoneNumber);
        }

        public async Task<Patient> GetPatientByPhoneAsync(string patientPhoneNumber)
        {
            return await Context.Patients.FirstOrDefaultAsync(p => p.Phone == patientPhoneNumber);
        }
    }
}
