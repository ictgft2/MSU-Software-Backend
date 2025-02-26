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
        public VitalService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        public async Task<VitalsResponseDto> CreateVitals(int patientId, VitalsRequestDto vitals)
        {
            var patient = _repository.Patient.FindByCondition(x => x.PatientId == patientId);

            if(patient is null)
            {
                
            }
            var newVitals = new Vital
            {
                BloodPressure = vitals.BloodPressure
            };
        }
    }
}
