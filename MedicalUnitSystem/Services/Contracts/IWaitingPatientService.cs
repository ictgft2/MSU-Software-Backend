using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IWaitingPatientService
    {
        Task<Result<WaitingPatientResponseDto>> CreateWaitingPatient(int patient);
        Task<Result<List<WaitingPatientResponseDto>>> GetWaitingPatientslist();
    }
}
