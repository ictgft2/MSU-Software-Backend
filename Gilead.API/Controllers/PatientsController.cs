using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[Route("api/v1/patients")]
public sealed class PatientsController(IPatientService patients) : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Register(RegisterPatientRequest request, CancellationToken cancellationToken)
    {
        var result = await patients.RegisterAsync(request, cancellationToken);
        return result.Succeeded
            ? CreatedAtAction(nameof(GetById), new { patientId = result.Data!.Id }, result)
            : FromResult(result);
    }

    [HttpGet("{patientId:guid}", Name = nameof(GetById))]
    public async Task<ActionResult> GetById(Guid patientId, CancellationToken cancellationToken) =>
        FromResult(await patients.GetByIdAsync(patientId, cancellationToken));

    [HttpGet("search")]
    public async Task<ActionResult> Search([FromQuery] string? name, [FromQuery] string? phone, CancellationToken cancellationToken) =>
        FromResult(await patients.SearchAsync(name, phone, cancellationToken));
}
