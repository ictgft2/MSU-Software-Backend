using AutoMapper;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class WaitPatientService : IWaitingPatientService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public WaitPatientService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<Result<WaitingPatientResponseDto>> CreateWaitingPatient(int patient)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<WaitingPatientResponseDto>>> GetWaitingPatientslist()
        {
            throw new NotImplementedException();
        }
    }
}
