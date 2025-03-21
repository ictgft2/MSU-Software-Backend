using AutoMapper;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Helpers.Enums;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MedicalUnitSystem.Services
{
    public class LaboratoryTestService : ILaboratoryTestService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly IPropertyCheckingService _propertyCheckingService;

        public LaboratoryTestService(IRepositoryWrapper repository, IMapper mapper, IPropertyCheckingService propertyCheckingService)
        {
            _repository = repository;
            _mapper = mapper;
            _propertyCheckingService = propertyCheckingService;
        }

        public Task<Result<CreateLaboratoryTestResponseDto>> CreateLaboratoryTest(CreateLaboratoryTestRequestDto laboratoryTest)
        {
            var newLaboratoryTest = new LaboratoryTest
            {
                LaboratoryTestTypeId = laboratoryTest.LaboratoryTestTypeId,
                PatientId = laboratoryTest.PatientId
            };

            _repository.LaboratoryTests.Create(newLaboratoryTest);

            _repository.Save();

            var response = _mapper.Map<CreateLaboratoryTestResponseDto>(newLaboratoryTest);

            return Task.FromResult(Result.Success<CreateLaboratoryTestResponseDto>(response));
        }

        public Task<Result<GetLaboratoryTestResponseDto>> GetLaboratoryTest(int laboratoryTestId)
        {
            var existingLaboratoryTestQuery = _repository.LaboratoryTests.FindByCondition(x => x.LaboratoryTestId == laboratoryTestId);

            var laboratoryTest = existingLaboratoryTestQuery.FirstOrDefault();

            if (laboratoryTest == null)
            {
                return Task.FromResult(Result.Failure<GetLaboratoryTestResponseDto>($"LaboratoryTest with Id:{laboratoryTestId} not found"));
            }

            var response = _mapper.Map<GetLaboratoryTestResponseDto>(laboratoryTest);

            return Task.FromResult(Result<GetLaboratoryTestResponseDto>.Success(response));
        }

        public async Task<PagedList<GetLaboratoryTestResponseDto>> GetLaboratoryTests(GetPaginatedDataRequestDto query)
        {
            IQueryable<LaboratoryTest> laboratoryTestsQuery = _repository.LaboratoryTests.FindAll();

            if (!string.IsNullOrWhiteSpace(query.searchTerm))
            {
                laboratoryTestsQuery = _repository.LaboratoryTests
                    .FindByCondition(d => d.LaboratoryTestType.LaboratoryTestTypeName.Contains(query.searchTerm) || 
                    d.Patient.Name.Contains(query.searchTerm));
            }

            var propertyInfo = _propertyCheckingService.CheckProperty<LaboratoryTest>(query.sortColumn);

            if (propertyInfo is not null)
            {
                if (query.sortOrder == SortOrder.Descending)
                {
                    laboratoryTestsQuery = laboratoryTestsQuery.OrderByDescending(d => propertyInfo.GetValue(d, null));
                }
                else
                {
                    laboratoryTestsQuery = laboratoryTestsQuery.OrderBy(d => propertyInfo.GetValue(d, null));
                }
            }

            var laboratoryTestResponsesQuery = laboratoryTestsQuery
                .Include(l => l.LaboratoryTestType)
                .Include(l => l.Patient)
                .Select(d => new GetLaboratoryTestResponseDto
                {
                    PatientId = d.PatientId,
                    LaboratoryTestId = d.LaboratoryTestId,
                    LaboratoryTestTypeId = d.LaboratoryTestTypeId
                });

            var laboratoryTests = await PagedList<GetLaboratoryTestResponseDto>.CreateAsync(laboratoryTestResponsesQuery, query.page, query.pageSize);

            return laboratoryTests;
        }

        public async Task<bool> LaboratoryTestExistsAsync(int laboratoryTestId)
        {
            return await _repository.LaboratoryTests.LaboratoryTestExistsAsync(laboratoryTestId);
        }

        public void UpdateLaboratoryTest(int laboratoryTestId, UpdateLaboratoryTestRequestDto laboratoryTestDetails)
        {
            var existingLaboratoryTest = _repository.LaboratoryTests.FindByCondition(x => x.LaboratoryTestId == laboratoryTestId);

            var laboratoryTest = existingLaboratoryTest.FirstOrDefault();

            laboratoryTest.LaboratoryTestTypeId = laboratoryTestDetails.LaboratoryTestTypeId;

            laboratoryTest.PatientId = laboratoryTestDetails.PatientId;

            _repository.LaboratoryTests.Update(laboratoryTest);

            _repository.Save();
        }
    }
}
