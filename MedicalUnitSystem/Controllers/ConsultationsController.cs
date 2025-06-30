using MedicalUnitSystem.DTOs.Enums;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MedicalUnitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public ConsultationsController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpPost("{doctorId}/{patientId}")]
        public async Task<ActionResult<Result<CreateConsultationResponseDto>>> CreateConsultation([FromRoute] int doctorId, [FromRoute]int patientId, [FromBody] CreateConsultationRequestDto consultation)
        {
            var newConsultation = await _serviceWrapper.Consultation.CreateConsultation(doctorId, patientId, consultation);

            if (!newConsultation.IsSuccess)
            {
                return BadRequest(newConsultation);
            }

            return CreatedAtRoute("GetConsultation",
                new { consultationId = newConsultation.Value.ConsultationId },
                newConsultation);
        }

        [HttpPut("{consultationId}")]
        public async Task<ActionResult> UpdateConsultation([FromRoute] int consultationId, [FromBody] UpdateConsultationRequestDto consultation)
        {
            if (!await _serviceWrapper.Consultation.ConsultationExistsAsync(consultationId))
            {
                return NotFound();
            }

            _serviceWrapper.Consultation.UpdateConsultation(consultationId, consultation);

            return NoContent();
        }

        [HttpGet(Name = "GetConsultations")]
        public async Task<ActionResult<Result<PagedList<GetConsultationResponseDto>>>> GetConsultations([FromQuery] GetPaginatedDataRequestDto query, [FromQuery] ConsultationEnum sortColumn = ConsultationEnum.ConsultationDate)
        {
            return Ok(await _serviceWrapper.Consultation.GetConsultations(sortColumn, query));
        }

        [HttpGet("{consultationId}", Name = "GetConsultation")]
        public async Task<ActionResult<Result<GetConsultationResponseDto>>> GetConsultation([FromRoute] int consultationId)
        {
            return Ok(await _serviceWrapper.Consultation.GetConsultation(consultationId));
        }
    }
}
