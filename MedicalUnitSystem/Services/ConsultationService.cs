using AutoMapper;
using MedicalUnitSystem.DTOs;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class ConsultationService : IConsultationService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public ConsultationService(IRepositoryWrapper repository, IMapper mapper) 
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public Task<Result<CreateConsultationResponseDto>> CreateConsultation(int patientId, CreateConsultationRequestDto consultation)
        {
            var patient = _repository.Patient.FindByCondition(x => x.PatientId == patientId);

            if (patient != null)
            {
                return Task.FromResult(Result.Failure<CreateConsultationResponseDto>($"Patient with Id:{patientId} not found"));
            }

            //var doctor = _repository.Doctor.FindByCondition(x => x.DoctorId == patientId);

            //if (doctor != null)
            //{
            //    return Task.FromResult(Result.Failure<CreateConsultationResponseDto>($"Patient with Id:{patientId} not found"));
            //}

            var newConsultation = new Consultation
            {
                Diagnosis = consultation.Diagnosis,
                Symptoms = consultation.Symptoms,
                Notes = consultation.Notes,  
                Patient = patient.FirstOrDefault()
            };

            if(consultation.Prescriptions.Count > 0)
            {
                foreach (var prescription in consultation.Prescriptions)
                {
                    var newPrescription = new Prescription
                    {
                        Dosage = prescription.Dosage,
                        Frequency = prescription.Frequency,
                        Instructions = prescription.Instructions,
                        MedicationName = prescription.MedicationName,
                        Consultation = newConsultation
                    };

                    newConsultation.Prescriptions.Add(newPrescription);
                    _repository.Prescription.Create(newPrescription);
                }
            }

           _repository.Consultation.Create(newConsultation);

            _repository.Save();

            var response = _mapper.Map<CreateConsultationResponseDto>(newConsultation);

            return Task.FromResult(Result.Success<CreateConsultationResponseDto>(response));
        }

        public Task<Result<CreateConsultationResponseDto>> UpdateConsultation(int patientId, UpdateConsultationRequestDto consultation)
        {
            throw new NotImplementedException();
        }
    }
}
