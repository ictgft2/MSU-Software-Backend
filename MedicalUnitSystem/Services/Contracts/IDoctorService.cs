using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IDoctorService
    {
        Task<Result<CreateDoctorResponseDto>> CreateDoctor(CreateDoctorRequestDto doctor);
        void UpdateDoctor(int doctorId, UpdateDoctorRequestDto doctor);
        Task<Result<GetDoctorResponseDto>> GetDoctor(int doctorId);
        Task<Result<List<GetDoctorResponseDto>>> GetDoctors();
        Task<bool> DoctorExistsAsync(int doctorId);
    }
}
