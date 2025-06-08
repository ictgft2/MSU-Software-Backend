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
    public class VitalService : IVitalsService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IPropertyCheckingService _propertyCheckingService;
        private readonly IMapper _mapper;

        public VitalService(IRepositoryWrapper repository, IMapper mapper, IPropertyCheckingService propertyCheckingService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyCheckingService = propertyCheckingService;
        }
        public Task<Result<CreateVitalsResponseDto>> CreateVitals(int patientId, VitalsRequestDto vitalDetails)
        {
            var existingPatientQuery = _repository.Patients.FindByCondition(x => x.PatientId == patientId);

            var patient = existingPatientQuery.FirstOrDefault();

            if(existingPatientQuery is null)
            {
                return Task.FromResult(Result.Failure<CreateVitalsResponseDto>($"Patient with Id:{patientId} not found"));
            }

            var newVitals = new Vital
            {
               HeartRate = vitalDetails.HeartRate,
               DiastolicBloodPressure = vitalDetails.DiastolicBloodPressure,
               Notes = vitalDetails.Notes,
               RespiratoryRate = vitalDetails.RespiratoryRate,
               SystolicBloodPressure = vitalDetails.SystolicBloodPressure,
               OxygenSaturation = vitalDetails.OxygenSaturation,
               Temperature = vitalDetails.Temperature,
               PatientId = patientId
            };

            _repository.Vitals.Create(newVitals);

            _repository.Save();

            var response = _mapper.Map<CreateVitalsResponseDto>(newVitals);

            return Task.FromResult(Result<CreateVitalsResponseDto>.Success(response));
        }

        public void UpdateVitals(int vitalId, UpdateVitalsRequestDto vitalsDetails)
        {
            var existingVitals = _repository.Vitals
                .FindByCondition(x => x.VitalsId == vitalId);

            var vitals = existingVitals.FirstOrDefault();

            vitals.DiastolicBloodPressure = vitalsDetails.DiastolicBloodPressure;
            vitals.SystolicBloodPressure = vitalsDetails.SystolicBloodPressure;
            vitals.Temperature = vitalsDetails.Temperature;
            vitals.HeartRate = vitalsDetails.HeartRate;
            vitals.Notes = vitalsDetails.Notes;
            vitals.OxygenSaturation = vitalsDetails.OxygenSaturation;
            vitals.RespiratoryRate = vitalsDetails.RespiratoryRate;

            _repository.Vitals.Update(vitals);

            _repository.Save();
        }

        public Task<Result<GetVitalsResponseDto>> GetVitals(int vitalsId)
        {
            var existingVitalsQuery = _repository.Vitals.FindByCondition(x => x.VitalsId == vitalsId);

            var vitals = existingVitalsQuery.FirstOrDefault();

            if (vitals == null)
            {
                return Task.FromResult(Result.Failure<GetVitalsResponseDto>($"Patient with Id:{vitalsId} not found"));
            }

            var response = _mapper.Map<GetVitalsResponseDto>(vitals);

            return Task.FromResult(Result<GetVitalsResponseDto>.Success(response));
        }

        public async Task<PagedList<GetVitalsResponseDto>> GetAllPatientVitals(GetPaginatedDataRequestDto query)
        {
            IQueryable<Vital> vitalsQuery = _repository.Vitals.FindAll();

            if (!string.IsNullOrWhiteSpace(query.searchTerm))
            {
                vitalsQuery = _repository.Vitals
                    .FindByCondition(d => d.Notes.Contains(query.searchTerm));
            }

            var propertyInfo = _propertyCheckingService.CheckProperty<Vital>(query.sortColumn);

            if (propertyInfo is not null)
            {
               vitalsQuery = vitalsQuery.OrderByProperty(propertyInfo, query.sortOrder);
            }

            var vitalsResponsesQuery = vitalsQuery
                .Select(d => new GetVitalsResponseDto
                {
                    OxygenSaturation = d.OxygenSaturation,
                    Notes = d.Notes,
                    RespiratoryRate = d.RespiratoryRate,
                    HeartRate = d.HeartRate,
                    DiastolicBloodPressure = d.DiastolicBloodPressure,
                    SystolicBloodPressure = d.SystolicBloodPressure,
                    Temperature = d.Temperature,
                    PatientId = d.PatientId
                });

            var patients = await PagedList<GetVitalsResponseDto>.CreateAsync(vitalsResponsesQuery, query.page, query.pageSize);

            return patients;
        }

        public async Task<bool> VitalsExistsAsync(int vitalsId)
        {
            return await _repository.Vitals.VitalsExistsAsync(vitalsId);
        }
    }
}
