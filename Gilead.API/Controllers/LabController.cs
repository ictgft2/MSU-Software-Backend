using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Gilead.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[Route("api/v1")]
public sealed class LabController(ILabService labs) : ApiControllerBase
{
    [HttpGet("lab/requests")]
    public async Task<ActionResult> Worklist([FromQuery] LabRequestStatus? status, [FromQuery] DateOnly? date, CancellationToken cancellationToken) => FromResult(await labs.GetRequestsAsync(status, date, cancellationToken));

    [HttpGet("lab/requests/{requestId:guid}")]
    public async Task<ActionResult> GetRequest(Guid requestId, CancellationToken cancellationToken) => FromResult(await labs.GetRequestAsync(requestId, cancellationToken));

    [HttpPost("lab/requests/{requestId:guid}/results")]
    public async Task<ActionResult> PostResult(Guid requestId, PostLabResultRequest request, CancellationToken cancellationToken) => FromResult(await labs.PostResultAsync(requestId, request, cancellationToken));

    [HttpGet("encounters/{encounterId:guid}/lab-results")]
    public async Task<ActionResult> Results(Guid encounterId, CancellationToken cancellationToken) => FromResult(await labs.GetResultsByEncounterAsync(encounterId, cancellationToken));
}
