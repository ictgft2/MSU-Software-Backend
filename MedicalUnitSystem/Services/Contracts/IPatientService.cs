using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IPatientService
    {
        Task<Result<CreatePatientResponseDto>> CreatePatient(CreatePatientRequestDto patient);
        void UpdatePatient(int patientId, UpdatePatientRequestDto patient);
        Task<Result<GetPatientResponseDto>> GetPatient(int patientId);
        Task<PagedList<GetPatientResponseDto>> GetPatients(GetPaginatedDataRequestDto query);
        Task<Result<CreateAdmissionResponseDto>> AdmitPatient(string patientPhoneNumber, bool PhoneNumberExists);
        Task<bool> PatientExistsAsync(int patientId);
        Task<bool> PatientExistsAsync(string patientPhoneNumber);
    }
}
