using MedicalUnitSystem.DTOs;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IConsultationService
    {
        Task<Result<CreateConsultationResponseDto>> CreateConsultation(int patientId, CreateConsultationRequestDto consultation);
        void UpdateConsultation( int consultationId, UpdateConsultationRequestDto consultation);
        Task<Result<GetConsultationResponseDto>> GetConsultation(int consultationId);
        Task<PagedList<GetConsultationResponseDto>> GetConsultations(GetPaginatedDataRequestDto query);
        Task<bool> ConsultationExistsAsync(int consultationId);
    }
}
