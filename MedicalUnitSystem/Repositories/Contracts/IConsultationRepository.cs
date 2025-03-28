﻿using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface IConsultationRepository : IRepository<Consultation>
    {
        Task<bool> ConsultationExistsAsync(int consultationId);
    }
}
