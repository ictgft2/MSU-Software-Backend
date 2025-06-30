using AutoMapper;
using MedicalUnitSystem.DTOs.Enums;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services.Contracts;

namespace MedicalUnitSystem.Services
{
    public class WaitingQueueService : IWaitingQueueService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IPropertyCheckingService _propertyCheckingService;
        private readonly IMapper _mapper;
        public WaitingQueueService(IRepositoryWrapper repository, IMapper mapper, IPropertyCheckingService propertyCheckingService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyCheckingService = propertyCheckingService;
        }

        public Task<Result<AddPatientToWaitingQueueResponseDto>> AddPatientToWaitingQueue(int patientId)
        {
            var existingPatientQuery = _repository.Patients.FindByCondition(x => x.PatientId == patientId);

            var patient = existingPatientQuery.FirstOrDefault();

            if (patient is null)
            {
                return Task.FromResult(Result<AddPatientToWaitingQueueResponseDto>.Failure($"Patient with Id:{patientId} not found"));
            }

            var newWaitingQueue = new WaitingQueue
            {
                PatientId = patientId,
                AttendedTo = false
            };

            _repository.WaitingQueues.Create(newWaitingQueue);

            _repository.Save();

            var response = _mapper.Map<AddPatientToWaitingQueueResponseDto>(newWaitingQueue);

            return Task.FromResult(Result<AddPatientToWaitingQueueResponseDto>.Success(response));
        }

        public Task<Result<GetWaitingQueueResponseDto>> GetWaitingQueue(int waitingQueueId)
        {
            var existingWaitingQueueQuery = _repository.WaitingQueues.FindByCondition(x => x.WaitingQueueId == waitingQueueId);

            var waitingQueue = existingWaitingQueueQuery.FirstOrDefault();

            if (waitingQueue == null)
            {
                return Task.FromResult(Result<GetWaitingQueueResponseDto>.Failure($"Waiting Queue with Id:{waitingQueueId} not found"));
            }

            var response = _mapper.Map<GetWaitingQueueResponseDto>(waitingQueue);

            return Task.FromResult(Result<GetWaitingQueueResponseDto>.Success(response));
        }

        public async Task<PagedList<GetWaitingQueueResponseDto>> GetWaitingQueues(WaitingQueueEnum sortColumn, GetPaginatedDataRequestDto query)
        {
            IQueryable<WaitingQueue> waitingQueuesQuery = _repository.WaitingQueues.FindAll();

            if (!string.IsNullOrWhiteSpace(query.searchTerm))
            {
                bool isValidBoolean = bool.TryParse(query.searchTerm, out bool result);

                if (isValidBoolean)
                {
                    waitingQueuesQuery = _repository.WaitingQueues
                        .FindByCondition(d => d.AttendedTo == result);
                }
            }

            var propertyInfo = _propertyCheckingService.CheckProperty<WaitingQueue>(sortColumn.ToString());

            if (propertyInfo is not null)
            {
               waitingQueuesQuery = waitingQueuesQuery.OrderByProperty(propertyInfo, query.sortOrder);
            }

            var waitingQueueResponsesQuery = waitingQueuesQuery
                .Select(d => new GetWaitingQueueResponseDto
                {
                    WaitingQueueId = d.WaitingQueueId,
                    AttendedTo = d.AttendedTo,
                    DateQueued = d.DateQueued,
                    PatientId = d.PatientId
                });

            var waitingQueues = await PagedList<GetWaitingQueueResponseDto>.CreateAsync(waitingQueueResponsesQuery, query.page, query.pageSize);

            return waitingQueues;
        }

        public void UpdateWaitingQueue(int waitingQueueId, UpdateWaitingQueueRequestDto waitingQueueDetails)
        {
            var existingWaitingQueue = _repository.WaitingQueues
                .FindByCondition(x => x.WaitingQueueId == waitingQueueId);

            var waitingQueue = existingWaitingQueue.FirstOrDefault();

            waitingQueue.AttendedTo = waitingQueueDetails.AttendedTo;
            waitingQueue.PatientId = waitingQueueDetails.PatientId;

            _repository.WaitingQueues.Update(waitingQueue);

            _repository.Save();
        }

        public async Task<bool> WaitingQueueExistsAsync(int waitingQueueId)
        {
            return await _repository.WaitingQueues.WaitingQueueExistsAsync(waitingQueueId);
        }
    }
}
