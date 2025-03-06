using AutoMapper;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class AdmissionService : IAdmissionService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public AdmissionService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public CreateAdmissionResponseDto AdmitPatient(int patientId)
        {
            var newAdmission = new Admission
            {
                PatientId = patientId,
                AdmissionTime = DateTimeOffset.UtcNow,
                IsDischarged = false
            };

            _repository.Admissions.Create(newAdmission);

            _repository.Save();

            return _mapper.Map<CreateAdmissionResponseDto>(newAdmission);
        }
    }
}
