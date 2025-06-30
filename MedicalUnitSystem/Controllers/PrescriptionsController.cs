using MedicalUnitSystem.DTOs.Requests;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MedicalUnitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public PrescriptionsController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        //[HttpPost("{dispense}")]
        //public async Task<ActionResult<Result>> DispenseDrugs([FromBody] DispenseDrugRequestDto dispenseDrug)
        //{
        //    foreach (var prescriptionId in dispenseDrug.Prescriptions)
        //    {
        //        if(!await _serviceWrapper.Prescription.PrescriptionExistsAsync(prescriptionId))
        //        {
        //            return NotFound($"Prescription with id:{prescriptionId} not found");
        //        }
        //    }

        //    return Ok(await _serviceWrapper.Prescription.DispenseDrugs(dispenseDrug.Prescriptions));
        //}
    }
}
