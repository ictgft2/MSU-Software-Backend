using AutoMapper;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class VitalService : IVitalsService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public VitalService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public Task<Result<VitalsResponseDto>> CreateVitals(int patientId, VitalsRequestDto vitals)
        {
            var patient = _repository.Patient.FindByCondition(x => x.PatientId == patientId);

            if(patient is null)
            {
                return Task.FromResult(Result.Failure<VitalsResponseDto>($"Patient with Id:{patientId} not found"));
            }

            var newVitals = new Vital
            {
                BloodPressure = vitals.BloodPressure,
                Patient = patient.FirstOrDefault(),
            };

            _repository.Vitals.Create(newVitals);

            _repository.Save();

            var response = _mapper.Map<VitalsResponseDto>(newVitals);

            return Task.FromResult(Result<VitalsResponseDto>.Success(response));
        }
    }
}
