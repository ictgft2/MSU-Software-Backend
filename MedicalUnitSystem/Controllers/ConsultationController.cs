using MedicalUnitSystem.DTOs;
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
        private readonly IConsultationService _consultationService;

        public ConsultationController(IConsultationService consultationService)
        {
            _consultationService = consultationService;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> CreateConsultation([FromQuery]int patientId, [FromBody] ConsultationDto consultation)
        {
            return Ok(await _consultationService.CreateConsultation(patientId, consultation));
        }
    }
}
