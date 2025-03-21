using MedicalUnitSystem.DTOs.Requests;
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
        public async Task<ActionResult<Result>> CreateLaboratoryTest([FromBody] CreateLaboratoryTestRequestDto laboratoryTest)
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

            return CreatedAtRoute("GetLaboratoryTest",
                new { laboratoryTestId = newLaboratoryTest.Data.LaboratoryTestId },
                newLaboratoryTest);
        }

        [HttpGet]
        public async Task<ActionResult<Result>> GetLaboratoryTests([FromQuery] GetPaginatedDataRequestDto query)
        {
            return Ok(await _serviceWrapper.LaboratoryTest.GetLaboratoryTests(query));
        }

        [HttpGet("{laboratoryTestId}", Name = "GetLaboratoryTest")]
        public async Task<ActionResult<Result>> GetLaboratoryTest([FromRoute] int laboratoryTestId)
        {
            return Ok(await _serviceWrapper.LaboratoryTest.GetLaboratoryTest(laboratoryTestId));
        }

        [HttpPut("{laboratoryTestId}")]
        public async Task<ActionResult<Result>> UpdateLaboratoryTest([FromRoute] int laboratoryTestId, [FromBody] UpdateLaboratoryTestRequestDto laboratoryTest)
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
