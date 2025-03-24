using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalUnitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdmissionsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;

        public AdmissionsController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }
    }
}
