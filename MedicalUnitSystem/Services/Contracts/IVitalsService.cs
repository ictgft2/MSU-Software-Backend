using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IVitalsService
    {
        Task<Result<VitalsResponseDto>> CreateVitals(int patientId, VitalsRequestDto vitals);
    }
}
