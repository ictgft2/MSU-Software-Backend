using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Gilead.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[Route("api/v1/dressing/orders")]
public sealed class DressingController(IDressingService dressing) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Worklist([FromQuery] DressingOrderStatus? status, CancellationToken cancellationToken) => FromResult(await dressing.GetWorklistAsync(status, cancellationToken));

    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult> Get(Guid orderId, CancellationToken cancellationToken) => FromResult(await dressing.GetByIdAsync(orderId, cancellationToken));

    [HttpPatch("{orderId:guid}/complete")]
    public async Task<ActionResult> Complete(Guid orderId, CompleteDressingOrderRequest request, CancellationToken cancellationToken) => FromResult(await dressing.CompleteAsync(orderId, request, cancellationToken));
}
