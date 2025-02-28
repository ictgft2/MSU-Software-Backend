using AutoMapper;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public PatientService(IRepositoryWrapper repository, IMapper mapper) 
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<Result<PatientResponseDto>> CreatePatient(PatientRequestDto patient)
        {
            var newPatient = new Patient
            {
                Age = patient.Age,
                Name = patient.Name,
                Gender = patient.Gender,
                ContactInfo = patient.ContactInfo,
                MedicalHistory = patient.MedicalHistory,
            };

            _repository.Patient.Create(newPatient);

            _repository.Save();

            var response = _mapper.Map<PatientResponseDto>(newPatient);

            return Task.FromResult(Result<PatientResponseDto>.Success(response));
        }
    }
}
