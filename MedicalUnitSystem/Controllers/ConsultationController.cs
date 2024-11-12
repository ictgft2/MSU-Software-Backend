using MedicalUnitSystem.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalUnitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        [HttpGet]
        public async Task Index()
        {
            throw new MedicalAppException(Constants.ConsultationNotFound, System.Net.HttpStatusCode.NotFound);
        }
    }
}
