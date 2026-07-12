using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[Route("api/v1/encounters/{encounterId:guid}/vitals")]
public sealed class VitalsController(IVitalsService vitals) : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Record(Guid encounterId, RecordVitalsRequest request, CancellationToken cancellationToken) => FromResult(await vitals.RecordAsync(encounterId, request, cancellationToken));

    [HttpGet]
    public async Task<ActionResult> GetAll(Guid encounterId, CancellationToken cancellationToken) => FromResult(await vitals.GetByEncounterAsync(encounterId, cancellationToken));

    [HttpGet("latest")]
    public async Task<ActionResult> Latest(Guid encounterId, CancellationToken cancellationToken) => FromResult(await vitals.GetLatestAsync(encounterId, cancellationToken));
}
