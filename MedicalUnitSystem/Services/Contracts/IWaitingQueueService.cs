using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;

namespace MedicalUnitSystem.Services.Contracts
{
    public interface IWaitingQueueService
    {
        Task<Result<AddPatientToWaitingQueueResponseDto>> AddPatientToWaitingQueue(int patientId);
        Task<Result<GetWaitingQueueResponseDto>> GetWaitingQueue(int waitingQueueId);
        void UpdateWaitingQueue(int waitingQueueId, UpdateWaitingQueueRequestDto waitingQueueDetails);
        Task<PagedList<GetWaitingQueueResponseDto>> GetWaitingQueues(GetPaginatedDataRequestDto query);
        Task<bool> WaitingQueueExistsAsync(int waitingQueueId);
    }
}
