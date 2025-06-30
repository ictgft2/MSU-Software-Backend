using AutoMapper;
using MedicalUnitSystem.DTOs.Enums;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MedicalUnitSystem.Services
{
    public class ConsultationService : IConsultationService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly IPropertyCheckingService _propertyCheckingService;
        public ConsultationService(IRepositoryWrapper repository, IMapper mapper, IPropertyCheckingService propertyCheckingService) 
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyCheckingService = propertyCheckingService;
        }

        public async Task<bool> ConsultationExistsAsync(int consultationId)
        {
            return await _repository.Consultations.ConsultationExistsAsync(consultationId);
        }

        public Task<Result<CreateConsultationResponseDto>> CreateConsultation(int doctorId, int patientId, CreateConsultationRequestDto consultation)
        {
            var existingPatientQuery = _repository.Patients.FindByCondition(x => x.PatientId == patientId);

            var patient = existingPatientQuery.FirstOrDefault();

            if (patient == null)
            {
                return Task.FromResult(Result<CreateConsultationResponseDto>.Failure($"Patient with Id:{patientId} not found"));
            }

            var existingDoctorQuery = _repository.Doctors.FindByCondition(x => x.DoctorId == doctorId);

            var doctor = existingDoctorQuery.FirstOrDefault();

            if (doctor == null)
            {
                return Task.FromResult(Result<CreateConsultationResponseDto>.Failure($"Doctor with Id:{doctorId} not found"));
            }

            using var transaction = _repository.Context.Database.BeginTransaction();

            try
            {
                var newConsultation = new Consultation
                {
                    Diagnosis = consultation.Diagnosis,
                    Symptoms = consultation.Symptoms,
                    Notes = consultation.Notes,  
                    PatientId = patientId,
                    DoctorId = doctorId,
                    ConsultationDate = DateTime.UtcNow,
                    FollowupDate = consultation.FollowupDate
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
                            ConsultationId = newConsultation.ConsultationId
                        };

                        newConsultation.Prescriptions.Add(newPrescription);
                        _repository.Prescriptions.Create(newPrescription);
                    }
                }

               _repository.Consultations.Create(newConsultation);

                _repository.Save();

                var prescriptionResponses = _mapper.Map<List<GetPrescriptionResponseDto>>(newConsultation.Prescriptions);

                var response = _mapper.Map<CreateConsultationResponseDto>(newConsultation);

                transaction.Commit();

                return Task.FromResult(Result<CreateConsultationResponseDto>.Success(response));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public Task<Result<GetConsultationResponseDto>> GetConsultation(int consultationId)
        {
            var existingConsultation = _repository.Consultations
                .FindByCondition(x => x.ConsultationId == consultationId)
                .Include(c => c.Patient)
                .Include(c => c.Doctor)
                .Include(c => c.Prescriptions);

            var consultation = existingConsultation.FirstOrDefault();

            if (consultation == null)
            {
                return Task.FromResult(Result<GetConsultationResponseDto>.Failure($"Consultation with Id:{consultationId} not found"));
            }

            var response = _mapper.Map<GetConsultationResponseDto>(consultation);

            return Task.FromResult(Result<GetConsultationResponseDto>.Success(response));
        }

        public async Task<Result<PagedList<GetConsultationResponseDto>>> GetConsultations(ConsultationEnum sortColumn, GetPaginatedDataRequestDto query)
        {
            IQueryable<Consultation> consultationsQuery = _repository.Consultations.FindAll();

            if (!string.IsNullOrWhiteSpace(query.searchTerm))
            {
                consultationsQuery = _repository.Consultations
                    .FindByCondition(d => d.Patient.Name.Contains(query.searchTerm) || 
                    d.Notes.Contains(query.searchTerm));
            }

            var propertyInfo = _propertyCheckingService.CheckProperty<Consultation>(sortColumn.ToString());

            if (propertyInfo is not null)
            {
                consultationsQuery = consultationsQuery.OrderByProperty(propertyInfo, query.sortOrder);
            }

            var consultationResponsesQuery = consultationsQuery
                .Select(d => new GetConsultationResponseDto
                {
                    ConsultationId = d.ConsultationId,
                    Notes = d.Notes,
                    ConsultationDate = d.ConsultationDate,
                    Diagnosis = d.Diagnosis,
                    Prescriptions = d.Prescriptions
                    .Select(p => new GetPrescriptionResponseDto
                    {
                        Dosage = p.Dosage,
                        Frequency = p.Frequency,
                        Instructions = p.Instructions,
                        MedicationName = p.MedicationName
                    }).ToList(),
                    FollowupDate = d.FollowupDate,
                    Symptoms = d.Symptoms
                });

            var consultations = await PagedList<GetConsultationResponseDto>.CreateAsync(consultationResponsesQuery, query.page, query.pageSize);

            return Result<PagedList<GetConsultationResponseDto>>.Success(consultations);
        }

        public void UpdateConsultation(int consultationId, UpdateConsultationRequestDto consultationDetails)
        {
            var existingConsultation = _repository.Consultations
                .FindByCondition(x => x.ConsultationId == consultationId)
                .Include(c => c.Prescriptions);

            var consultation = existingConsultation.FirstOrDefault();

            // clear existing prescriptions
            consultation.Prescriptions.Clear();
            // map new prescriptions
            var newPrescriptions = _mapper.Map<List<Prescription>>(consultationDetails.Prescriptions);
            consultation.Prescriptions = newPrescriptions;
            consultation.FollowupDate = consultationDetails.FollowupDate;
            consultation.Notes = consultationDetails.Notes;
            consultation.Diagnosis = consultationDetails.Diagnosis;
            consultation.Symptoms = consultationDetails.Symptoms;

            _repository.Consultations.Update(consultation);

            _repository.Save();
        }
    }
}
