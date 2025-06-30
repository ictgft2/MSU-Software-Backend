using MedicalUnitSystem.DTOs.Enums;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IVitalsService
    {
        Task<Result<CreateVitalsResponseDto>> CreateVitals(int patientId, VitalsRequestDto vitals);
        void UpdateVitals(int vitalId, UpdateVitalsRequestDto updateVitals);
        Task<bool> VitalsExistsAsync(int vitalsId);
        Task<Result<GetVitalsResponseDto>> GetVitals(int vitalsId);
        Task<PagedList<GetVitalsResponseDto>> GetAllPatientVitals(VitalsEnum sortColumn, GetPaginatedDataRequestDto query);
    }
}
