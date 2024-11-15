using MedicalUnitSystem.Repositories;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class ServiceWrapper : IServiceWrapper
    {
        private IRepositoryWrapper _repository;
        private IPatientService _patient;

        public IPatientService Patient
        {
            get
            {
                if(_patient == null)
                {
                    _patient = new PatientService(_repository);
                }

                return _patient;
            }
        }

        public ServiceWrapper(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
    }
}
