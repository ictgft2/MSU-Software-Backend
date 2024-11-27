using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class LaboratoryTestService : ILaboratoryTestService
    {
        private readonly IRepositoryWrapper _repository;

        public LaboratoryTestService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
    }
}
