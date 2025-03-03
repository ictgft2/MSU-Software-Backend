using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MedicalUnitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public DoctorsController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> CreateDoctor([FromBody] CreateDoctorRequestDto doctor)
        {
            return Ok(await _serviceWrapper.Doctor.CreateDoctor(doctor));
        }

        [HttpGet]
        public async Task<ActionResult<Result>> GetDoctors()
        {
            return Ok(await _serviceWrapper.Doctor.GetDoctors());
        }

        [HttpGet("{doctorId}")]
        public async Task<ActionResult<Result>> GetDoctor([FromRoute] int doctorId)
        {
            return Ok(await _serviceWrapper.Doctor.GetDoctor(doctorId));
        }

        [HttpPatch("{doctorId}")]
        public async Task<ActionResult<Result>> UpdateDoctor([FromRoute] int doctorId, [FromBody] UpdateDoctorRequestDto doctor)
        {
            return Ok(await _serviceWrapper.Doctor.UpdateDoctor(doctorId, doctor));
        }
    }
}
