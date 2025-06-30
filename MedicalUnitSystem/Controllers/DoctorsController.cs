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
    public class DoctorsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public DoctorsController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpPost]
        public async Task<ActionResult<Result<CreateDoctorResponseDto>>> CreateDoctor([FromBody] CreateDoctorRequestDto doctor)
        {
            var newDoctor = await _serviceWrapper.Doctor.CreateDoctor(doctor);

            if (!newDoctor.IsSuccess)
            {
                return BadRequest(newDoctor);
            }

            return CreatedAtRoute("GetDoctor", 
                new { doctorId = newDoctor.Value.DoctorId },
                newDoctor);
        }

        [HttpGet(Name = "GetDoctors")]
        public async Task<ActionResult<Result<PagedList<GetDoctorResponseDto>>>> GetDoctors( [FromQuery] GetPaginatedDataRequestDto query, [FromQuery] DoctorEnum sortColumn = DoctorEnum.Name)
        {
            return Ok(await _serviceWrapper.Doctor.GetDoctors(sortColumn, query));
        }

        [HttpGet("{doctorId}", Name = "GetDoctor")]
        public async Task<ActionResult<Result<GetDoctorResponseDto>>> GetDoctor([FromRoute] int doctorId)
        {
            return Ok(await _serviceWrapper.Doctor.GetDoctor(doctorId));
        }

        [HttpPut("{doctorId}")]
        public async Task<ActionResult> UpdateDoctor([FromRoute] int doctorId, [FromBody] UpdateDoctorRequestDto doctor)
        {
            if(!await _serviceWrapper.Doctor.DoctorExistsAsync(doctorId))
            {
                return NotFound();
            }

            _serviceWrapper.Doctor.UpdateDoctor(doctorId, doctor);

            return NoContent();
        }
    }
}
