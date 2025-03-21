using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface ILaboratoryTestTypeService
    {
        Task<Result<CreateLaboratoryTestTypeResponseDto>> CreateLaboratoryTestType(CreateLaboratoryTestTypeRequestDto laboratoryTestType);
        void UpdateLaboratoryTestType(int laboratoryTestTypeId, UpdateLaboratoryTestTypeRequestDto laboratoryTestType);
        Task<Result<GetLaboratoryTestTypeResponseDto>> GetLaboratoryTestType(int laboratoryTestTypeId);
        Task<PagedList<GetLaboratoryTestTypeResponseDto>> GetLaboratoryTestTypes(GetPaginatedDataRequestDto query);
        Task<bool> LaboratoryTestTypeExistsAsync(int laboratoryTestTypeId);
    }
}
