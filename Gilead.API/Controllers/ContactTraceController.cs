using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[Route("api/v1/encounters/{encounterId:guid}/contact-trace")]
public sealed class ContactTraceController(IContactTraceService contactTrace) : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Record(Guid encounterId, ContactTraceRequest request, CancellationToken cancellationToken) => FromResult(await contactTrace.RecordAsync(encounterId, request, cancellationToken));

    [HttpGet]
    public async Task<ActionResult> Get(Guid encounterId, CancellationToken cancellationToken) => FromResult(await contactTrace.GetAsync(encounterId, cancellationToken));

    [HttpPatch]
    public async Task<ActionResult> Update(Guid encounterId, ContactTraceRequest request, CancellationToken cancellationToken) => FromResult(await contactTrace.UpdateAsync(encounterId, request, cancellationToken));
}
