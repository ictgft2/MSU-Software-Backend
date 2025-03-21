using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IDoctorService
    {
        Task<Result<CreateDoctorResponseDto>> CreateDoctor(CreateDoctorRequestDto doctor);
        void UpdateDoctor(int doctorId, UpdateDoctorRequestDto doctor);
        Task<Result<GetDoctorResponseDto>> GetDoctor(int doctorId);
        Task<PagedList<GetDoctorResponseDto>> GetDoctors(GetPaginatedDataRequestDto query);
        Task<bool> DoctorExistsAsync(int doctorId);
    }
}
