using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[Route("api/v1/protocol/handovers")]
public sealed class ProtocolController(IProtocolService protocol) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Worklist([FromQuery] string? status, CancellationToken cancellationToken) => FromResult(await protocol.GetWorklistAsync(status, cancellationToken));

    [HttpGet("{handoverId:guid}")]
    public async Task<ActionResult> Get(Guid handoverId, CancellationToken cancellationToken) => FromResult(await protocol.GetByIdAsync(handoverId, cancellationToken));

    [HttpPost("{handoverId:guid}/confirm")]
    public async Task<ActionResult> Confirm(Guid handoverId, ConfirmHandoverRequest request, CancellationToken cancellationToken) => FromResult(await protocol.ConfirmAsync(handoverId, request, cancellationToken));
}
