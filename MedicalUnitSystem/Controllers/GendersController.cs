using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MedicalUnitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GendersController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public GendersController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpPost]        
        public async Task<ActionResult<Result>> CreateGender([FromBody] CreateGenderRequestDto gender)
        {
            return Ok(await _serviceWrapper.Gender.CreateGender(gender));
        }

        [HttpGet]
        public async Task<ActionResult<Result>> GetGenders()
        {
            return Ok(await _serviceWrapper.Gender.GetGenders());
        }

        [HttpGet("{genderId}")]
        public async Task<ActionResult<Result>> GetGender([FromRoute] int genderId)
        {
            return Ok(await _serviceWrapper.Gender.GetGender(genderId));
        }

        [HttpPut("{genderId}")]
        public async Task<ActionResult<Result>> UpdateDoctor([FromRoute] int genderId, [FromBody] UpdateGenderRequestDto gender)
        {
            if (!await _serviceWrapper.Gender.GenderExistsAsync(genderId))
            {
                return NotFound();
            }

             await _serviceWrapper.Gender.UpdateGender(genderId, gender);

            return NoContent();
        }
    }
}
