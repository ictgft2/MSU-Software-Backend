using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IPrescriptionService
    {
        Prescription DispenseDrug(int prescriptionId);
        Task<Result<DispenseDrugsResponseDto>> DispenseDrugs(List<int> prescriptions);
        Task<bool> PrescriptionExistsAsync(int prescriptionId);
    }
}
