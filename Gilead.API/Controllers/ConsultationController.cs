using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[Route("api/v1/encounters/{encounterId:guid}/consultation")]
public sealed class ConsultationController(IConsultationService consultations) : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Submit(Guid encounterId, SubmitConsultationRequest request, CancellationToken cancellationToken) => FromResult(await consultations.SubmitAsync(encounterId, request, cancellationToken));

    [HttpGet]
    public async Task<ActionResult> Get(Guid encounterId, CancellationToken cancellationToken) => FromResult(await consultations.GetByEncounterAsync(encounterId, cancellationToken));
}
