using MedicalUnitSystem.Data;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MedicalUnitSystem.Repositories
{
    public class WaitngQueueRepository : Repository<WaitingQueue>, IWaitingQueueRepository
    {
        public WaitngQueueRepository(HospitalContext context) : base(context)
        {
        }

        public async Task<bool> WaitingQueueExistsAsync(int waitingQueueId)
        {
            if (waitingQueueId is 0 || waitingQueueId is int.MinValue)
            {
                throw new ArgumentNullException(nameof(waitingQueueId));
            }

            return await Context.WaitingQueues.AnyAsync(d => d.WaitingQueueId == waitingQueueId);
        }
    }
}
