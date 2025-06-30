using MedicalUnitSystem.DTOs.Enums;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MedicalUnitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaboratoryTestsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public LaboratoryTestsController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpPost]
        public async Task<ActionResult<Result<CreateLaboratoryTestResponseDto>>> CreateLaboratoryTest([FromBody] CreateLaboratoryTestRequestDto laboratoryTest)
        {
            if(!await _serviceWrapper.LaboratoryTestType.LaboratoryTestTypeExistsAsync(laboratoryTest.LaboratoryTestTypeId))
            {
                return NotFound("Laboratory Test Type not found");
            }

            if(!await _serviceWrapper.Patient.PatientExistsAsync(laboratoryTest.PatientId))
            {
                return NotFound("Patient not found");
            }

            var newLaboratoryTest = await _serviceWrapper.LaboratoryTest.CreateLaboratoryTest(laboratoryTest);

            if (!newLaboratoryTest.IsSuccess)
            {
                return BadRequest(newLaboratoryTest);
            }

            return CreatedAtRoute("GetLaboratoryTest",
                new { laboratoryTestId = newLaboratoryTest.Value.LaboratoryTestId },
                newLaboratoryTest);
        }

        [HttpGet]
        public async Task<ActionResult<Result<GetLaboratoryTestResponseDto>>> GetLaboratoryTests( [FromQuery] GetPaginatedDataRequestDto query, [FromQuery] LaboratoryTestEnum sortColumn = LaboratoryTestEnum.LaboratoryTestTypeId)
        {
            return Ok(await _serviceWrapper.LaboratoryTest.GetLaboratoryTests(sortColumn, query));
        }

        [HttpGet("{laboratoryTestId}", Name = "GetLaboratoryTest")]
        public async Task<ActionResult<Result<GetLaboratoryTestResponseDto>>> GetLaboratoryTest([FromRoute] int laboratoryTestId)
        {
            return Ok(await _serviceWrapper.LaboratoryTest.GetLaboratoryTest(laboratoryTestId));
        }

        [HttpPut("{laboratoryTestId}")]
        public async Task<ActionResult> UpdateLaboratoryTest([FromRoute] int laboratoryTestId, [FromBody] UpdateLaboratoryTestRequestDto laboratoryTest)
        {
            if (!await _serviceWrapper.LaboratoryTest.LaboratoryTestExistsAsync(laboratoryTestId))
            {
                return NotFound();
            }

            _serviceWrapper.LaboratoryTest.UpdateLaboratoryTest(laboratoryTestId, laboratoryTest);

            return NoContent();
        }
    }
}
