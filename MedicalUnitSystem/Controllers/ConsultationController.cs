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
    public class ConsultationController : ControllerBase
    {
        private readonly IServiceWrapper _service;

        public ConsultationController(IServiceWrapper service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> CreateConsultation([FromQuery]int patientId, [FromBody] ConsultationRequestDto consultation)
        {
            return Ok(await _service.Consultation.CreateConsultation(patientId, consultation));
        }
    }
}
