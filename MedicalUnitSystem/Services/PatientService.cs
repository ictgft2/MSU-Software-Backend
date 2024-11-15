using MedicalUnitSystem.DTOs;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepositoryWrapper _repository;
        public PatientService(IRepositoryWrapper repository) 
        {
            _repository = repository;
        }

        public Task<Result<Patient>> CreatePatient(PatientDto patient)
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

            return Task.FromResult(Result<Patient>.Success(newPatient));
        }
    }
}
