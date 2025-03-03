using MedicalUnitSystem.DTOs;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IConsultationService
    {
        Task<Result<CreateConsultationResponseDto>> CreateConsultation(int patientId, CreateConsultationRequestDto consultation);
        Task<Result<CreateConsultationResponseDto>> UpdateConsultation(int patientId, UpdateConsultationRequestDto consultation);
    }
}
