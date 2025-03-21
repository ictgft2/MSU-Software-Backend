using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface ILaboratoryTestService
    {
        Task<Result<CreateLaboratoryTestResponseDto>> CreateLaboratoryTest(CreateLaboratoryTestRequestDto laboratoryTest);
        void UpdateLaboratoryTest(int laboratoryTestId, UpdateLaboratoryTestRequestDto laboratoryTest);
        Task<Result<GetLaboratoryTestResponseDto>> GetLaboratoryTest(int laboratoryTestId);
        Task<PagedList<GetLaboratoryTestResponseDto>> GetLaboratoryTests(GetPaginatedDataRequestDto query);
        Task<bool> LaboratoryTestExistsAsync(int laboratoryTestId);
    }
}
