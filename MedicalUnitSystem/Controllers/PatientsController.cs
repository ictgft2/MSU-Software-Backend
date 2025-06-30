using MedicalUnitSystem.DTOs.Enums;
using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.DTOs.Responses;
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
        public async Task<ActionResult<Result<CreatePatientResponseDto>>> CreatePatient([FromBody] CreatePatientRequestDto patient)
        {
            var newPatient = await _serviceWrapper.Patient.CreatePatient(patient);

            if (!newPatient.IsSuccess)
            {
                return BadRequest(newPatient);
            }

            return CreatedAtRoute("GetPatient", 
                new { patientId = newPatient.Value.PatientId },
                newPatient);
        }

        [HttpPost("admit")]
        public async Task<ActionResult<Result<CreateVitalsResponseDto>>> AdmitPatient([FromBody] CreateAdmissionRequestDto admission)
        {
            var patientExists = await _serviceWrapper.Patient.PatientExistsAsync(admission.PatientPhoneNumber);

            var admittedPatient = await _serviceWrapper.Patient.AdmitPatient(
                admission.PatientPhoneNumber, patientExists);

            return CreatedAtRoute("GetPatient",
                new { patientId = admittedPatient.Value.PatientId },
                    admittedPatient);
        }

        [HttpPost("discharge")]
        public async Task<ActionResult<Result<DischargePatientResponseDto>>> DischargePatient([FromBody] DischargePatientRequestDto dischargePatient)
        {
            if(!await _serviceWrapper.Patient.PatientExistsAsync(dischargePatient.PatientPhoneNumber))
            {
                return NotFound("Patient Not Found");
            }

            return Ok(await _serviceWrapper.Patient.DischargePatient(dischargePatient));
        }

        [HttpGet("{patientId}", Name = "GetPatient")]
        public async Task<ActionResult<Result<GetPatientResponseDto>>> GetPatient([FromRoute] int patientId)
        {
            return Ok(await _serviceWrapper.Patient.GetPatient(patientId));
        }

        [HttpGet(Name = "GetPatients")]
        public async Task<ActionResult<Result<PagedList<GetPatientResponseDto>>>> GetPatients( [FromQuery] GetPaginatedDataRequestDto query, [FromQuery] PatientEnum sortColumn = PatientEnum.Name)
        {
            return Ok(await _serviceWrapper.Patient.GetPatients(sortColumn, query));
        }

        [HttpPut("{patientId}")]
        public async Task<ActionResult> UpdatePatient([FromRoute] int patientId, [FromBody] UpdatePatientRequestDto patient)
        {
            if(!await _serviceWrapper.Patient.PatientExistsAsync(patientId))
            {
                return NotFound();
            }

            var result = _serviceWrapper.Patient.UpdatePatient(patientId, patient);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPut("vitals/{vitalsId}")]
        public async Task<ActionResult> UpdateVitals([FromRoute] int vitalsId, [FromBody] UpdateVitalsRequestDto vitals)
        {
            if(!await _serviceWrapper.Vitals.VitalsExistsAsync(vitalsId))
            {
                return NotFound();
            }

            _serviceWrapper.Vitals.UpdateVitals(vitalsId, vitals);

            return NoContent();
        }

        [HttpPost("vitals/{patientId}")]
        public async Task<ActionResult<Result<CreateVitalsResponseDto>>> CreatePatientVitals([FromRoute] int patientId, [FromBody] VitalsRequestDto vitals)
        {
            var newPatientVitals = await _serviceWrapper.Vitals.CreateVitals(patientId, vitals);
            return CreatedAtRoute("GetPatientVitals",
               new { patientId = newPatientVitals.Value.PatientId },
               newPatientVitals);
        }

        [HttpGet("vitals/{vitalsId}", Name = "GetPatientVitals")]
        public async Task<ActionResult<Result<GetVitalsResponseDto>>> GetPatientVitals([FromRoute] int vitalsId)
        {
            return Ok(await _serviceWrapper.Vitals.GetVitals(vitalsId));
        }

        [HttpGet("vitals")]
        public async Task<ActionResult<Result<PagedList<GetVitalsResponseDto>>>> GetAllPatientVitals([FromQuery] GetPaginatedDataRequestDto query, [FromQuery] VitalsEnum sortColumn)
        {
            return Ok(await _serviceWrapper.Vitals.GetAllPatientVitals(sortColumn, query));
        }
    }
}
