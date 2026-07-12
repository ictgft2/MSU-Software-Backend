using Gilead.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[Route("api/v1/queue")]
public sealed class QueueController(IQueueService queue) : ApiControllerBase
{
    [HttpPost("{encounterId:guid}/join")]
    public async Task<ActionResult> Join(Guid encounterId, CancellationToken cancellationToken) => FromResult(await queue.JoinAsync(encounterId, cancellationToken));

    [HttpDelete("{encounterId:guid}")]
    public async Task<ActionResult> Dequeue(Guid encounterId, CancellationToken cancellationToken) => FromResult(await queue.DequeueAsync(encounterId, cancellationToken));

    [HttpGet]
    public async Task<ActionResult> GetFullList(CancellationToken cancellationToken) => FromResult(await queue.GetFullListAsync(cancellationToken));

    [HttpGet("{encounterId:guid}/position")]
    public async Task<ActionResult> GetPosition(Guid encounterId, CancellationToken cancellationToken) => FromResult(await queue.GetPositionAsync(encounterId, cancellationToken));
}
