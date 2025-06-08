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
    public class PatientService : IPatientService
    {
        #region Properties
        private readonly IRepositoryWrapper _repository;
        private readonly IPropertyCheckingService _propertyCheckingService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public PatientService(IRepositoryWrapper repository, IMapper mapper, IPropertyCheckingService propertyCheckingService) 
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyCheckingService = propertyCheckingService;
        }
        #endregion

        #region Methods
        public Task<Result<CreatePatientResponseDto>> CreatePatient(CreatePatientRequestDto patient)
        {
            var newPatient = MakePatient(patient);            

            var response = _mapper.Map<CreatePatientResponseDto>(newPatient);

            return Task.FromResult(Result<CreatePatientResponseDto>.Success(response));
        }

        private Patient MakePatient(CreatePatientRequestDto patient) 
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

            _repository.Patients.Create(newPatient);

            _repository.Save();

            return newPatient;
        }

        public void UpdatePatient(int patientId, UpdatePatientRequestDto patientDetails)
        {
            var existingPatient = _repository.Patients.FindByCondition(x => x.PatientId == patientId);

            var patient = existingPatient.FirstOrDefault();

            patient.Name = patientDetails.Name;
            patient.Age = patientDetails.Age;
            patient.Email = patientDetails.Email;
            patient.Phone = patientDetails.Phone;
            patient.GenderId = patientDetails.GenderId;

            _repository.Patients.Update(patient);

            _repository.Save();
        }

        public Task<Result<GetPatientResponseDto>> GetPatient(int patientId)
        {
            var existingPatientQuery = _repository.Patients.FindByCondition(x => x.PatientId == patientId);

            var patient = existingPatientQuery.FirstOrDefault();

            if(patient == null)
            {
                return Task.FromResult(Result.Failure<GetPatientResponseDto>($"Patient with Id:{patientId} not found"));
            }

            var response = _mapper.Map<GetPatientResponseDto>(patient);

            return Task.FromResult(Result<GetPatientResponseDto>.Success(response));
        }

        public Task<Result<DischargePatientResponseDto>> DischargePatient(DischargePatientRequestDto discharge)
        {
            var existingPatientQuery = _repository.Patients.FindByCondition(p => p.Phone == discharge.PatientPhoneNumber);

            var patient = existingPatientQuery.FirstOrDefault();

            if (patient == null)
            {
                return Task.FromResult(Result.Failure<DischargePatientResponseDto>($"Patient with phone number:{discharge.PatientPhoneNumber} not found"));
            }

            var admissionDetailsQuery = _repository.Admissions.FindByCondition(a => a.PatientId == patient.PatientId & a.IsDischarged == true);

            var admission = admissionDetailsQuery.FirstOrDefault();

            if(admission is not null)
            {
                admission.IsDischarged = true;
                admission.DischargeNotes = discharge.DischargeNotes;

                _repository.Admissions.Update(admission);

                _repository.Save();

                var response = _mapper.Map<DischargePatientResponseDto>(admission);

                return Task.FromResult(Result.Success<DischargePatientResponseDto>(response));
            }

            return Task.FromResult(Result.Failure<DischargePatientResponseDto>($"No Admission Record for Patient with Patient Number:{patient.PatientNumber}"));
        }

        public void AdmitPatient(int patientId)
        {
            var existingAdmittedPatientQuery = _repository.Admissions.FindByCondition(a => a.PatientId == patientId & a.IsDischarged == false);

            var admittedPatient = existingAdmittedPatientQuery.FirstOrDefault();

            if (admittedPatient == null)
            {
                throw new Exception("Patient is currently admitted");
            }

            var newAdmission = new Admission
            {
                PatientId = patientId,
                AdmissionTime = DateTimeOffset.UtcNow,
                IsDischarged = false
            };

            _repository.Admissions.Create(newAdmission);

            _repository.Save();
        }

        public async Task<Result<CreatePatientResponseDto>> AdmitPatient(string patientPhoneNumber, bool phoneNumberExists)
        {
            // check if the patient phone number exist
            // if it does, return the existing patient
            // if not create a new patient with the patient phone number
            var patient = phoneNumberExists ?
               await _repository.Patients
               .FindByCondition(x => x.Phone == patientPhoneNumber)
               .FirstOrDefaultAsync() :
                MakePatient(
                    new CreatePatientRequestDto
                    {
                        Age = 0,
                        Email = "",
                        Name = "",
                        GenderId = 1,
                        Phone = patientPhoneNumber
                    });

            AdmitPatient(patient.PatientId);

            var response = _mapper.Map<CreatePatientResponseDto>(patient);

            return Result<CreatePatientResponseDto>.Success(response);
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

            } while (_repository.Patients.PatientNumberExist(patientNumber)); // Ensure uniqueness in DB

            return patientNumber;
        }
        public async Task<bool> PatientExistsAsync(int patientId)
        {
            return await _repository.Patients.PatientExistsAsync(patientId);
        }
        public async Task<bool> PatientExistsAsync(string patientPhoneNumber)
        {
            return await _repository.Patients.PatientExistsAsync(patientPhoneNumber);
        }

        public async Task<PagedList<GetPatientResponseDto>> GetPatients(GetPaginatedDataRequestDto query)
        {
            IQueryable<Patient> patientsQuery = _repository.Patients.FindAll();

            if (!string.IsNullOrWhiteSpace(query.searchTerm))
            {
                patientsQuery = _repository.Patients
                    .FindByCondition(d => d.Name.Contains(query.searchTerm) ||
                    d.PatientNumber.Contains(query.searchTerm));
            }

            var propertyInfo = _propertyCheckingService.CheckProperty<Patient>(query.sortColumn);

            if (propertyInfo is not null)
            {
                patientsQuery = patientsQuery.OrderByProperty(propertyInfo, query.sortOrder);
            }

            var patientResponsesQuery = patientsQuery
                .Select(d => new GetPatientResponseDto
                {
                    Name = d.Name,
                    Age = d.Age,
                    Email = d.Email,
                    MedicalHistory = d.MedicalHistory,
                    PatientId = d.PatientId,
                    Phone = d.Phone
                });

            var patients = await PagedList<GetPatientResponseDto>.CreateAsync(patientResponsesQuery, query.page, query.pageSize);

            return patients;
        }

        #endregion
    }
}
