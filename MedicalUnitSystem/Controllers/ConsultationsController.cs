using MedicalUnitSystem.DTOs;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Services;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("{patientId}")]
        public async Task<ActionResult<Result>> CreateConsultation([FromRoute]int patientId, [FromBody] CreateConsultationRequestDto consultation)
        {
            var newConsultation = await _serviceWrapper.Consultation.CreateConsultation(patientId, consultation);

            return CreatedAtRoute("GetConsultation",
                new { doctorId = newConsultation.Data.ConsultationId },
                newConsultation);
        }

        [HttpPut("{consultationId}")]
        public async Task<ActionResult<Result>> UpdateConsultation([FromRoute] int consultationId, [FromBody] UpdateConsultationRequestDto consultation)
        {
            if (!await _serviceWrapper.Consultation.ConsultationExistsAsync(consultationId))
            {
                return NotFound();
            }

            _serviceWrapper.Consultation.UpdateConsultation(consultationId, consultation);

            return NoContent();
        }

        [HttpGet(Name = "GetConsultations")]
        public async Task<ActionResult<Result>> GetConsultations([FromQuery] GetPaginatedDataRequestDto query)
        {
            return Ok(await _serviceWrapper.Consultation.GetConsultations(query));
        }

        [HttpGet("{consulatationId}", Name = "GetConsultation")]
        public async Task<ActionResult<Result>> GetConsultation([FromRoute] int consultationId)
        {
            return Ok(await _serviceWrapper.Consultation.GetConsultation(consultationId));
        }
    }
}
