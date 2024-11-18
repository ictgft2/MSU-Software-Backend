using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class LaboratoryTestTypeService : ILaboratoryTestTypeService
    {
        private readonly IRepositoryWrapper _repository;

        public LaboratoryTestTypeService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
    }
}
