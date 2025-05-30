﻿using MedicalUnitSystem.DTOs.Requests;
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
            var newDoctor = await _serviceWrapper.Doctor.CreateDoctor(doctor);

            return CreatedAtRoute("GetDoctor", 
                new { doctorId = newDoctor.Data.DoctorId },
                newDoctor);
        }

        [HttpGet(Name = "GetDoctors")]
        public async Task<ActionResult<Result>> GetDoctors([FromQuery] GetPaginatedDataRequestDto query)
        {
            return Ok(await _serviceWrapper.Doctor.GetDoctors(query));
        }

        [HttpGet("{doctorId}", Name = "GetDoctor")]
        public async Task<ActionResult<Result>> GetDoctor([FromRoute] int doctorId)
        {
            return Ok(await _serviceWrapper.Doctor.GetDoctor(doctorId));
        }

        [HttpPut("{doctorId}")]
        public async Task<ActionResult<Result>> UpdateDoctor([FromRoute] int doctorId, [FromBody] UpdateDoctorRequestDto doctor)
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
