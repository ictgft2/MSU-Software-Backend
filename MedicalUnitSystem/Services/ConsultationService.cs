using MedicalUnitSystem.DTOs;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class ConsultationService : IConsultationService
    {
        private readonly IRepositoryWrapper _repository;
        public ConsultationService(IRepositoryWrapper repository) 
        {
            _repository = repository;
        }
        public Task<Result<Consultation>> CreateConsultation(int patientId, ConsultationDto consultation)
        {
            var newConsultation = new Consultation
            {
                BloodPressure = consultation.BloodPressure,
                Diagnosis = consultation.Diagnosis,
                LaboratoryTests = consultation.LaboratoryTests,
                PatientId = patientId,
                Prescriptions = consultation.Prescriptions,
            };

           _repository.Consultation.Create(newConsultation);

            _repository.Save();

            return Task.FromResult(Result.Success<Consultation>(newConsultation));
        }
    }
}
