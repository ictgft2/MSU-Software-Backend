using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IGenderService
    {
        Task<Result<CreateGenderResponseDto>> CreateGender(CreateGenderRequestDto gender);
        Task<Result<UpdateGenderResponseDto>> UpdateGender(int genderId, UpdateGenderRequestDto gender);
        Task<Result<GetGenderResponseDto>> GetGender(int genderId);
        Task<Result<List<GetGenderResponseDto>>> GetGenders();
        Task<bool> GenderExistsAsync(int genderId);
    }
}
