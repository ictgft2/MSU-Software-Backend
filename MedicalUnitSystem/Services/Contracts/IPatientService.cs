using MedicalUnitSystem.DTOs;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IPatientService
    {
        Task<Result<Patient>> CreatePatient(PatientDto patient);
    }
}
