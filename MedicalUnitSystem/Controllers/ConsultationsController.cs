using MedicalUnitSystem.DTOs;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalUnitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationsController : ControllerBase
    {
        private readonly IServiceWrapper _service;

        public ConsultationsController(IServiceWrapper service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> CreateConsultation([FromQuery]int patientId, [FromBody] CreateConsultationRequestDto consultation)
        {
            return Ok(await _service.Consultation.CreateConsultation(patientId, consultation));
        }

        [HttpPatch("{consultationId}")]
        public async Task<ActionResult<Result>> UpdateConsultation([FromQuery]int patientId, [FromBody] UpdateConsultationRequestDto consultation)
        {
            return Ok(await _service.Consultation.UpdateConsultation(patientId, consultation));
        }
    }
}
