using MedicalUnitSystem.DTOs.Enums;
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
        Task<Result<PagedList<GetLaboratoryTestTypeResponseDto>>> GetLaboratoryTestTypes(LaboratoryTestTypeEnum sortColumn, GetPaginatedDataRequestDto query);
        Task<bool> LaboratoryTestTypeExistsAsync(int laboratoryTestTypeId);
    }
}
