using MedicalUnitSystem.DTOs;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IConsultationService
    {
        Task<Result<Consultation>> CreateConsultation(int patientId, ConsultationDto consultation);
    }
}
