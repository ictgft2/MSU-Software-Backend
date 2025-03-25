using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MedicalUnitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaitingQueuesController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public WaitingQueuesController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpPost("{patientId}")]
        public async Task<ActionResult<Result>> AddPatientToWaitingQueue([FromRoute] int patientId)
        {
            var newWaitingQueue = await _serviceWrapper.WaitingQueue.AddPatientToWaitingQueue(patientId);

            return CreatedAtRoute("GetWaitingQueue",
                new { waitingQueueId = newWaitingQueue.Data.WaitingQueueId },
                newWaitingQueue);
        }

        [HttpPut("{waitingQueueId}")]
        public async Task<ActionResult<Result>> UpdateConsultation([FromRoute] int waitingQueueId, [FromBody] UpdateWaitingQueueRequestDto waitingQueue)
        {
            if (!await _serviceWrapper.WaitingQueue.WaitingQueueExistsAsync(waitingQueueId))
            {
                return NotFound();
            }

            _serviceWrapper.WaitingQueue.UpdateWaitingQueue(waitingQueueId, waitingQueue);

            return NoContent();
        }

        [HttpGet(Name = "GetWaitingQueues")]
        public async Task<ActionResult<Result>> GetWaitingQueues([FromQuery] GetPaginatedDataRequestDto query)
        {
            return Ok(await _serviceWrapper.WaitingQueue.GetWaitingQueues(query));
        }

        [HttpGet("{waitingQueueId}", Name = "GetWaitingQueue")]
        public async Task<ActionResult<Result>> GetWaitingQueue([FromRoute] int waitingQueueId)
        {
            return Ok(await _serviceWrapper.WaitingQueue.GetWaitingQueue(waitingQueueId));
        }
    }
}
