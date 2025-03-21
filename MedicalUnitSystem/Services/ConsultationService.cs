using AutoMapper;
using MedicalUnitSystem.DTOs;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Helpers.Enums;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

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

        public Task<bool> ConsultationExistsAsync(int consultationId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<CreateConsultationResponseDto>> CreateConsultation(int patientId, CreateConsultationRequestDto consultation)
        {
            var existingPatientQuery = _repository.Patients.FindByCondition(x => x.PatientId == patientId);

            var patient = existingPatientQuery.FirstOrDefault();

            if (patient == null)
            {
                return Task.FromResult(Result.Failure<CreateConsultationResponseDto>($"Patient with Id:{patientId} not found"));
            }

            using var transaction = _repository.Context.Database.BeginTransaction();

            try
            {
                var newConsultation = new Consultation
                {
                    Diagnosis = consultation.Diagnosis,
                    Symptoms = consultation.Symptoms,
                    Notes = consultation.Notes,  
                    Patient = patient
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
                        _repository.Prescriptions.Create(newPrescription);
                    }
                }

               _repository.Consultations.Create(newConsultation);

                _repository.Save();

                var response = _mapper.Map<CreateConsultationResponseDto>(newConsultation);

                return Task.FromResult(Result.Success<CreateConsultationResponseDto>(response));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public Task<Result<GetConsultationResponseDto>> GetConsultation(int consultationId)
        {
            var existingConsultation = _repository.Consultations.FindByCondition(x => x.ConsultationId == consultationId);

            if (existingConsultation == null)
            {
                return Task.FromResult(Result.Failure<GetConsultationResponseDto>($"Consultation with Id:{consultationId} not found"));
            }

            var response = _mapper.Map<GetConsultationResponseDto>(existingConsultation);

            return Task.FromResult(Result<GetConsultationResponseDto>.Success(response));
        }

        public async Task<PagedList<GetConsultationResponseDto>> GetConsultations(GetPaginatedDataRequestDto query)
        {
            IQueryable<Consultation> consultationsQuery = _repository.Consultations.FindAll();

            if (!string.IsNullOrWhiteSpace(query.searchTerm))
            {
                consultationsQuery = _repository.Consultations
                    .FindByCondition(d => d.Patient.Name.Contains(query.searchTerm) || 
                    d.Notes.Contains(query.searchTerm));
            }

            var propertyInfo = _propertyCheckingService.CheckProperty<Consultation>(query.sortColumn);

            if (propertyInfo is not null)
            {
                if (query.sortOrder == SortOrder.Descending)
                {
                    consultationsQuery = consultationsQuery.OrderByDescending(d => propertyInfo.GetValue(d, null));
                }
                else
                {
                    consultationsQuery = consultationsQuery.OrderBy(d => propertyInfo.GetValue(d, null));
                }
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

            return consultations;
        }

        public void UpdateConsultation(int consultationId, UpdateConsultationRequestDto consultationDetails)
        {
            var existingConsultation = _repository.Consultations.FindByCondition(x => x.ConsultationId == consultationId);

            var consultation = existingConsultation.FirstOrDefault();

            consultation.Prescriptions = consultationDetails.Prescriptions
                .Select(p => new Prescription
                {
                    Dosage = p.Dosage,
                    Frequency = p.Frequency,
                    Instructions = p.Instructions,
                    MedicationName = p.MedicationName
                }).ToList();
            consultation.ConsultationDate = consultationDetails.ConsultationDate;
            consultation.FollowupDate = consultationDetails.FollowupDate;
            consultation.Notes = consultationDetails.Notes;
            consultation.Diagnosis = consultationDetails.Diagnosis;
            consultation.Symptoms = consultationDetails.Symptoms;

            _repository.Consultations.Update(consultation);

            _repository.Save();
        }
    }
}
