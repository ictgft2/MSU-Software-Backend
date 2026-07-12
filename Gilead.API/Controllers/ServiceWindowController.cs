using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[Route("api/v1/service-window")]
public sealed class ServiceWindowController(IServiceWindowService windows) : ApiControllerBase
{
    [HttpGet("current")]
    public async Task<ActionResult> Current(CancellationToken cancellationToken) => FromResult(await windows.GetCurrentAsync(cancellationToken));

    [HttpPost]
    public async Task<ActionResult> Set(SetServiceWindowRequest request, CancellationToken cancellationToken) => FromResult(await windows.SetTodayAsync(request, cancellationToken));

    [HttpPatch("{windowId:guid}")]
    public async Task<ActionResult> Update(Guid windowId, UpdateServiceWindowRequest request, CancellationToken cancellationToken) => FromResult(await windows.UpdateAsync(windowId, request, cancellationToken));
}
