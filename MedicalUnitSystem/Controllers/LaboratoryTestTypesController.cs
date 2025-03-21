using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MedicalUnitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaboratoryTestTypesController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public LaboratoryTestTypesController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> CreateLaboratoryTestType([FromBody] CreateLaboratoryTestTypeRequestDto laboratoryTestType)
        {
            var newLaboratoryTestType = await _serviceWrapper.LaboratoryTestType.CreateLaboratoryTestType(laboratoryTestType);

            return CreatedAtRoute("GetLaboratoryTestType",
                new { laboratoryTestTypeId = newLaboratoryTestType.Data.LaboratoryTestTypeId },
                newLaboratoryTestType);
        }

        [HttpGet("{laboratoryTestTypeId}", Name = "GetLaboratoryTestType")]
        public async Task<ActionResult<Result>> GetLaboratoryTestType([FromRoute] int laboratoryTestTypeId)
        {
            return Ok(await _serviceWrapper.LaboratoryTestType.GetLaboratoryTestType(laboratoryTestTypeId));
        }

        [HttpGet(Name = "GetLaboratoryTestTypes")]
        public async Task<ActionResult<Result>> GetLaboratoryTestTypes([FromRoute] GetPaginatedDataRequestDto query)
        {
            return Ok(await _serviceWrapper.LaboratoryTestType.GetLaboratoryTestTypes(query));
        }

        [HttpPut("{laboratoryTestTypeId}")]
        public async Task<ActionResult<Result>> UpdateLaboratoryTestType([FromRoute] int laboratoryTestTypeId, [FromBody] UpdateLaboratoryTestTypeRequestDto laboratoryTestType)
        {
            if (!await _serviceWrapper.LaboratoryTestType.LaboratoryTestTypeExistsAsync(laboratoryTestTypeId))
            {
                return NotFound();
            }

            _serviceWrapper.LaboratoryTestType.UpdateLaboratoryTestType(laboratoryTestTypeId, laboratoryTestType);

            return NoContent();
        }
    }
}
