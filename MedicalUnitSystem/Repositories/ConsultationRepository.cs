using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MedicalUnitSystem.Repositories
{
    public class ConsultationRepository : Repository<Consultation>, IConsultationRepository
    {
        public ConsultationRepository(HospitalContext context): base(context) { }
        public async Task<bool> ConsulatationExistsAsync(int consultationId)
        {
            if (consultationId is 0 || consultationId is int.MinValue)
            {
                throw new ArgumentNullException(nameof(consultationId));
            }

            return await Context.Consultations.AnyAsync(d => d.ConsultationId == consultationId);
        }
    }
}
