using AutoMapper;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public PrescriptionService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public Prescription DispenseDrug(int prescriptionId)
        {
            var prescriptionQuery = _repository.Prescriptions.FindByCondition(p => p.PrescriptionId == prescriptionId);

            var prescription = prescriptionQuery.FirstOrDefault();

            prescription.IsDispensed = true;

            _repository.Prescriptions.Update(prescription);

            _repository.Save();

            return prescription;
        }

        public Task<Result<DispenseDrugsResponseDto>> DispenseDrugs(List<int> prescriptions)
        {
            var dispenseDrugs = new DispenseDrugsResponseDto();

            foreach (var prescriptionId in prescriptions)
            {
                var prescription = DispenseDrug(prescriptionId);

                dispenseDrugs.DispenseDrugMessages.Add(
                    $"{prescription.MedicationName} has been dispense with instruction - {prescription.Instructions}"
                    );
            }

            return Task.FromResult(Result.Success<DispenseDrugsResponseDto>(dispenseDrugs));
        }

        public async Task<bool> PrescriptionExistsAsync(int prescriptionId)
        {
            return await _repository.Prescriptions.PrescriptionExistsAsync(prescriptionId);
        }
    }
}
