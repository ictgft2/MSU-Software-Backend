using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Gilead.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[Route("api/v1/encounters")]
public sealed class EncountersController(IEncounterService encounters) : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Open(OpenEncounterRequest request, CancellationToken cancellationToken)
    {
        var result = await encounters.OpenAsync(request, cancellationToken);
        return result.Succeeded
            ? CreatedAtAction(nameof(GetDetail), new { encounterId = result.Data!.Id }, result)
            : FromResult(result);
    }

    [HttpGet("{encounterId:guid}", Name = nameof(GetDetail))]
    public async Task<ActionResult> GetDetail(Guid encounterId, CancellationToken cancellationToken) =>
        FromResult(await encounters.GetDetailAsync(encounterId, cancellationToken));

    [HttpGet]
    public async Task<ActionResult> List([FromQuery] EncounterStatus? status, [FromQuery] DateOnly? date, [FromQuery] AdmissionType? type, CancellationToken cancellationToken) =>
        FromResult(await encounters.ListAsync(status, date, type, cancellationToken));

    [HttpPatch("{encounterId:guid}/status")]
    public async Task<ActionResult> AdvanceStatus(Guid encounterId, AdvanceEncounterStatusRequest request, CancellationToken cancellationToken) =>
        FromResult(await encounters.AdvanceStatusAsync(encounterId, request.Status, cancellationToken));
}
