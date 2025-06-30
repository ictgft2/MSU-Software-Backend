using MedicalUnitSystem.DTOs.Enums;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IConsultationService
    {
        Task<Result<CreateConsultationResponseDto>> CreateConsultation(int doctorId, int patientId, CreateConsultationRequestDto consultation);
        void UpdateConsultation( int consultationId, UpdateConsultationRequestDto consultation);
        Task<Result<GetConsultationResponseDto>> GetConsultation(int consultationId);
        Task<Result<PagedList<GetConsultationResponseDto>>> GetConsultations(ConsultationEnum sortColumn, GetPaginatedDataRequestDto query);
        Task<bool> ConsultationExistsAsync(int consultationId);
    }
}
