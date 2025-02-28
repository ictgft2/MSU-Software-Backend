using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalUnitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public PatientController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> CreatePatient([FromBody] PatientRequestDto patient)
        {
            return Ok(await _serviceWrapper.Patient.CreatePatient(patient));
        }

        [HttpPost("vitals/{patientId}")]
        public async Task<ActionResult<Result>> CreateVitals([FromRoute] int patientId, [FromBody] VitalsRequestDto vitals)
        {
            return Ok(await _serviceWrapper.Vitals.CreateVitals(patientId, vitals));
        }

        [HttpPost("waitlist/{patientId}")]
        public async Task<ActionResult<Result>> CreateWaitingPatient([FromRoute] int patientId)
        {
            return Ok(await _serviceWrapper.WaitingPatient.CreateWaitingPatient(patientId));
        }

        [HttpPost("waitlist")]
        public async Task<ActionResult<Result>> Waitlist()
        {
            return Ok(await _serviceWrapper.WaitingPatient.GetWaitingPatientslist());
        }
    }
}
