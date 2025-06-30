using AutoMapper;
using MedicalUnitSystem.DTOs.Enums;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Helpers.Enums;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace MedicalUnitSystem.Services
{
    public class LaboratoryTestTypeService : ILaboratoryTestTypeService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly IPropertyCheckingService _propertyCheckingService;

        public LaboratoryTestTypeService(IRepositoryWrapper repository, IMapper mapper, IPropertyCheckingService propertyCheckingService)
        {
            _repository = repository;
            _mapper = mapper;
            _propertyCheckingService = propertyCheckingService;
        }

        #region Methods
        public Task<Result<CreateLaboratoryTestTypeResponseDto>> CreateLaboratoryTestType(CreateLaboratoryTestTypeRequestDto request)
        {
            var laboratoryTestTypeQuery = _repository.LaboratoryTestTypes
                .FindByCondition(l => l.LaboratoryTestTypeName == request.LaboratoryTestTypeName);

            var laboratoryTestType = laboratoryTestTypeQuery.FirstOrDefault();

            if(laboratoryTestType is not null)
            {
               return Task.FromResult(Result<CreateLaboratoryTestTypeResponseDto>.Failure($"Laboratory Test Type:{request.LaboratoryTestTypeName} already exists"));               
            }

            var newLaboratoryTestType = new LaboratoryTestType
            {
                LaboratoryTestTypeName = request.LaboratoryTestTypeName,
            };

            _repository.LaboratoryTestTypes.Create(newLaboratoryTestType);

            _repository.Save();

            var response = _mapper.Map<CreateLaboratoryTestTypeResponseDto>(newLaboratoryTestType);

            return Task.FromResult(Result<CreateLaboratoryTestTypeResponseDto>.Success(response));
        }

        public Task<Result<GetLaboratoryTestTypeResponseDto>> GetLaboratoryTestType(int laboratoryTestTypeId)
        {
            var existingLaboratoryTestTypeQuery = _repository.LaboratoryTestTypes.FindByCondition(x => x.LaboratoryTestTypeId == laboratoryTestTypeId);

            var laboratoryTestType = existingLaboratoryTestTypeQuery.FirstOrDefault();

            if (laboratoryTestType == null)
            {
                return Task.FromResult(Result<GetLaboratoryTestTypeResponseDto>.Failure($"LaboratoryTestType with Id:{laboratoryTestTypeId} not found"));
            }   

            var response = _mapper.Map<GetLaboratoryTestTypeResponseDto>(laboratoryTestType);

            return Task.FromResult(Result<GetLaboratoryTestTypeResponseDto>.Success(response));
        }

        public async Task<Result<PagedList<GetLaboratoryTestTypeResponseDto>>> GetLaboratoryTestTypes(LaboratoryTestTypeEnum sortColumn, GetPaginatedDataRequestDto query)
        {
            IQueryable<LaboratoryTestType> laboratoryTestTypesQuery = _repository.LaboratoryTestTypes.FindAll();

            if (!string.IsNullOrWhiteSpace(query.searchTerm))
            {
                laboratoryTestTypesQuery = _repository.LaboratoryTestTypes
                    .FindByCondition(d => d.LaboratoryTestTypeName.Contains(query.searchTerm));
            }

            var propertyInfo = _propertyCheckingService.CheckProperty<LaboratoryTestType>(sortColumn.ToString());

            if (propertyInfo is not null)
            {
                laboratoryTestTypesQuery = laboratoryTestTypesQuery.OrderByProperty(propertyInfo, query.sortOrder); 
            }

            var laboratoryTestTypeResponsesQuery = laboratoryTestTypesQuery
                .Select(d => new GetLaboratoryTestTypeResponseDto
                {
                    LaboratoryTestTypeId = d.LaboratoryTestTypeId,
                    LaboratoryTestTypeName = d.LaboratoryTestTypeName                    
                });

            var laboratoryTestTypes = await PagedList<GetLaboratoryTestTypeResponseDto>.CreateAsync(laboratoryTestTypeResponsesQuery, query.page, query.pageSize);

            return Result<PagedList<GetLaboratoryTestTypeResponseDto>>.Success(laboratoryTestTypes);
        }

        public async Task<bool> LaboratoryTestTypeExistsAsync(int laboratoryTestTypeId)
        {
            return await _repository.LaboratoryTestTypes.LaboratoryTestTypeExistsAsync(laboratoryTestTypeId);
        }

        public void UpdateLaboratoryTestType(int laboratoryTestTypeId, UpdateLaboratoryTestTypeRequestDto laboratoryTestTypeDetails)
        {
            var existingLaboratoryTestType = _repository.LaboratoryTestTypes.FindByCondition(x => x.LaboratoryTestTypeId == laboratoryTestTypeId);

            var laboratoryTestType = existingLaboratoryTestType.FirstOrDefault();

            laboratoryTestType.LaboratoryTestTypeName = laboratoryTestTypeDetails.LaboratoryTestTypeName;

            _repository.LaboratoryTestTypes.Update(laboratoryTestType);

            _repository.Save();
        }
        #endregion
    }
}
