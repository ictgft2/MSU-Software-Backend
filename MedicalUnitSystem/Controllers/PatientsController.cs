using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalUnitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public PatientsController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> CreatePatient([FromBody] CreatePatientRequestDto patient)
        {
            var newPatient = await _serviceWrapper.Patient.CreatePatient(patient);

            return CreatedAtRoute("GetPatient", 
                new { patientId = newPatient.Data.PatientId },
                newPatient);
        }

        [HttpPost("admit")]
        public async Task<ActionResult<Result>> AdmitPatient([FromBody] string patientPhoneNumber)
        {
            var patientExists = await _serviceWrapper.Patient.PatientExistsAsync(patientPhoneNumber);

            var admittedPatient = await _serviceWrapper.Patient.AdmitPatient(
                patientPhoneNumber, patientExists);

            return CreatedAtRoute("GetPatient",
                new { patientId = admittedPatient.Data.PatientId },
                    admittedPatient);
        }

        [HttpGet("{patientId}", Name = "GetPatient")]
        public async Task<ActionResult<Result>> GetPatient([FromRoute] int patientId)
        {
            return Ok(await _serviceWrapper.Patient.GetPatient(patientId));
        }

        [HttpGet(Name = "GetPatients")]
        public async Task<ActionResult<Result>> GetPatients([FromQuery] GetPaginatedDataRequestDto query)
        {
            return Ok(await _serviceWrapper.Patient.GetPatients(query));
        }

        [HttpPut("{patientId}")]
        public async Task<ActionResult<Result>> UpdatePatient([FromRoute] int patientId, [FromBody] UpdatePatientRequestDto patient)
        {
            if(!await _serviceWrapper.Patient.PatientExistsAsync(patientId))
            {
                return NotFound();
            }

            _serviceWrapper.Patient.UpdatePatient(patientId, patient);

            return NoContent();
        }

        [HttpPost("vitals/{patientId}")]
        public async Task<ActionResult<Result>> CreateVitals([FromRoute] int patientId, [FromBody] VitalsRequestDto vitals)
        {
            return Ok(await _serviceWrapper.Vitals.CreateVitals(patientId, vitals));
        }

        [HttpPost("waitingpatient/{patientId}")]
        public async Task<ActionResult<Result>> CreateWaitingPatient([FromRoute] int patientId)
        {
            return Ok(await _serviceWrapper.WaitingPatient.CreateWaitingPatient(patientId));
        }

        [HttpGet("waitingpatients")]
        public async Task<ActionResult<Result>> WaitingPatients()
        {
            return Ok(await _serviceWrapper.WaitingPatient.GetWaitingPatientslist());
        }
    }
}
