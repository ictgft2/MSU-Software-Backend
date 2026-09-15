using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Gilead.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[Route("api/v1/staff")]
[Authorize]
public sealed class StaffController(IStaffService staff) : ApiControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Registrar")]
    public async Task<ActionResult> Create(CreateStaffRequest request, CancellationToken cancellationToken) =>
        FromResult(await staff.CreateAsync(request, cancellationToken));

    [HttpGet]
    public async Task<ActionResult> List([FromQuery] StaffRole? role, [FromQuery] bool? isActive, CancellationToken cancellationToken) =>
        FromResult(await staff.GetListAsync(role, isActive, cancellationToken));

    [HttpGet("{staffId:guid}")]
    public async Task<ActionResult> Get(Guid staffId, CancellationToken cancellationToken) =>
        FromResult(await staff.GetByIdAsync(staffId, cancellationToken));

    [HttpPatch("{staffId:guid}")]
    [Authorize(Roles = "Registrar")]
    public async Task<ActionResult> Update(Guid staffId, UpdateStaffRequest request, CancellationToken cancellationToken) =>
        FromResult(await staff.UpdateAsync(staffId, request, cancellationToken));
}