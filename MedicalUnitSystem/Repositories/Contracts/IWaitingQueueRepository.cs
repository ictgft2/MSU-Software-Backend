using MedicalUnitSystem.Models;

namespace MedicalUnitSystem.Repositories.Contracts
{
    public interface IWaitingQueueRepository : IRepository<WaitingQueue>
    {
        Task<bool> WaitingQueueExistsAsync(int waitingQueueId);
    }
}
