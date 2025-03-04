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

        public Task<Result<CreatePatientResponseDto>> CreatePatient(CreatePatientRequestDto patient)
        {
            var newPatientNumber = GenerateUniquePatientNumber();

            var newPatient = new Patient
            {
                Age = patient.Age,
                Name = patient.Name,
                GenderId = patient.GenderId,
                Email = patient.Email,
                PatientNumber = newPatientNumber,
                Phone = patient.Phone,
                MedicalHistory = patient.MedicalHistory,
            };

            _repository.Patient.Create(newPatient);

            _repository.Save();

            var response = _mapper.Map<CreatePatientResponseDto>(newPatient);

            return Task.FromResult(Result<CreatePatientResponseDto>.Success(response));
        }

        public void UpdatePatient(int patientId, UpdatePatientRequestDto patientDetails)
        {
            var existingPatient = _repository.Patient.FindByCondition(x => x.PatientId == patientId);

            var patient = existingPatient.FirstOrDefault();

            patient.Name = patientDetails.Name;
            patient.Age = patientDetails.Age;
            patient.Email = patientDetails.Email;
            patient.Phone = patientDetails.Phone;
            patient.GenderId = patientDetails.GenderId;

            _repository.Patient.Update(patient);

            _repository.Save();
        }

        public Task<Result<GetPatientResponseDto>> GetPatient(int patientId)
        {
            var existingPatient = _repository.Patient.FindByCondition(x => x.PatientId == patientId);

            if(existingPatient != null)
            {
                return Task.FromResult(Result.Failure<GetPatientResponseDto>($"Patient with Id:{patientId} not found"));
            }

            var response = _mapper.Map<GetPatientResponseDto>(existingPatient);

            return Task.FromResult(Result<GetPatientResponseDto>.Success(response));
        }

        private string GenerateUniquePatientNumber()
        {
            Random random = new Random();
            string patientNumber;

            do
            {
                // Generate 3 random uppercase letters
                string letters = new string(Enumerable.Range(0, 3)
                    .Select(_ => (char)random.Next('A', 'Z' + 1)).ToArray());

                // Generate 4 random digits
                string numbers = random.Next(1000, 9999).ToString();

                // Combine letters and numbers
                patientNumber = letters + numbers;

            } while (_repository.Patient.PatientNumberExist(patientNumber)); // Ensure uniqueness in DB

            return patientNumber;
        }

        public async Task<bool> PatientExistsAsync(int patientId)
        {
            return await _repository.Patient.PatientExistsAsync(patientId);
        }
    }
}
