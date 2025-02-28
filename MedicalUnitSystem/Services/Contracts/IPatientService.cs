using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IPatientService
    {
        Task<Result<PatientResponseDto>> CreatePatient(PatientRequestDto patient);
    }
}
