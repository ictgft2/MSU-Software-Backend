using AutoMapper;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class GenderService : IGenderService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public GenderService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public Task<Result<CreateGenderResponseDto>> CreateGender(CreateGenderRequestDto gender)
        {
            var newGender = new Gender
            {
                GenderName = gender.GenderName
            };

            _repository.Genders.Create(newGender);

            _repository.Save();

            var response = _mapper.Map<CreateGenderResponseDto>(newGender);

            return Task.FromResult(Result.Success<CreateGenderResponseDto>(response));
        }

        public async Task<bool> GenderExistsAsync(int genderId)
        {
            return await _repository.Genders.GenderExistsAsync(genderId);
        }

        public Task<Result<GetGenderResponseDto>> GetGender(int genderId)
        {
            var existingGenderQuery = _repository.Genders.FindByCondition(x => x.GenderId == genderId);

            var gender = existingGenderQuery.FirstOrDefault();

            if (gender == null)
            {
                return Task.FromResult(Result.Failure<GetGenderResponseDto>($"Gender with Id:{genderId} not found"));
            }

            var response = _mapper.Map<GetGenderResponseDto>(gender);
                
            return Task.FromResult(Result<GetGenderResponseDto>.Success(response));
        }

        public Task<Result<List<GetGenderResponseDto>>> GetGenders()
        {
            var genders = _repository.Genders.FindAll();

            var response = _mapper.Map<List<GetGenderResponseDto>>(genders.ToList());

            return Task.FromResult(Result<List<GetGenderResponseDto>>.Success(response));
        }

        public Task<Result<UpdateGenderResponseDto>> UpdateGender(int genderId, UpdateGenderRequestDto genderDetails)
        {
            var existingGender = _repository.Genders.FindByCondition(x => x.GenderId == genderId);

            if (existingGender == null)
            {
                return Task.FromResult(Result.Failure<UpdateGenderResponseDto>($"Gender with Id:{genderId} not found"));
            }

            var updatedGender = existingGender.FirstOrDefault();

           updatedGender.GenderName = genderDetails.GenderName;

            _repository.Genders.Update(updatedGender);

            _repository.Save();

            var response = _mapper.Map<UpdateGenderResponseDto>(updatedGender);

            return Task.FromResult(Result<UpdateGenderResponseDto>.Success(response));
        }
    }
}
