using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Gilead.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[Route("api/v1/pharmacy/prescriptions")]
public sealed class PharmacyController(IPharmacyService pharmacy) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Worklist([FromQuery] PrescriptionStatus? status, [FromQuery] DateOnly? date, CancellationToken cancellationToken) => FromResult(await pharmacy.GetWorklistAsync(status, date, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get(Guid id, CancellationToken cancellationToken) => FromResult(await pharmacy.GetByIdAsync(id, cancellationToken));

    [HttpPost("{id:guid}/dispense")]
    public async Task<ActionResult> Dispense(Guid id, DispensePrescriptionRequest request, CancellationToken cancellationToken) => FromResult(await pharmacy.DispenseAsync(id, request, cancellationToken));
}
