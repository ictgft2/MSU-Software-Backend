using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IAdmissionService
    {
        CreateAdmissionResponseDto AdmitPatient(int patientId);
    }
}
