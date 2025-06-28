using MedicalUnitSystem.DTOs.Enums;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IPatientService
    {
        Task<Result<CreatePatientResponseDto>> CreatePatient(CreatePatientRequestDto patient);
        Result<UpdatePatientResponseDto> UpdatePatient(int patientId, UpdatePatientRequestDto patient);
        Task<Result<GetPatientResponseDto>> GetPatient(int patientId);
        Task<PagedList<GetPatientResponseDto>> GetPatients(PatientEnum sortColumn, GetPaginatedDataRequestDto query);
        Task<Result<CreatePatientResponseDto>> AdmitPatient(string patientPhoneNumber, bool PhoneNumberExists);
        Task<Result<DischargePatientResponseDto>> DischargePatient(DischargePatientRequestDto discharge);
        Task<bool> PatientExistsAsync(int patientId);
        Task<bool> PatientExistsAsync(string patientPhoneNumber);
    }
}
