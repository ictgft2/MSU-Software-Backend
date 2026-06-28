using Gilead.Application;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult FromResult<T>(ServiceResult<T> result, string? actionName = null, object? routeValues = null)
    {
        if (!result.Succeeded)
            return StatusCode(result.StatusCode, result);
        if (result.StatusCode == StatusCodes.Status201Created)
            return actionName is null || routeValues is null
                ? Created(string.Empty, result)
                : CreatedAtAction(actionName, routeValues, result);
        return Ok(result);
    }

    protected ActionResult FromResult(ServiceResult result)
    {
        if (!result.Succeeded)
            return StatusCode(result.StatusCode, result);
        return Ok(result);
    }
}
