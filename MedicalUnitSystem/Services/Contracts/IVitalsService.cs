using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IVitalsService
    {
        Task<VitalsResponseDto> CreateVitals(int patientId, VitalsRequestDto vitals);
    }
}
