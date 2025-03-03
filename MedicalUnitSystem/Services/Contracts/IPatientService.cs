using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IPatientService
    {
        Task<Result<CreatePatientResponseDto>> CreatePatient(CreatePatientRequestDto patient);
        Task<Result<UpdatePatientResponseDto>> UpdatePatient(int patientId, UpdatePatientRequestDto patient);
        Task<Result<GetPatientResponseDto>> GetPatient(int patientId);
    }
}
