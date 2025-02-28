using MedicalUnitSystem.DTOs;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IConsultationService
    {
        Task<Result<Consultation>> CreateConsultation(int patientId, ConsultationRequestDto consultation);
    }
}
