using Gilead.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gilead.API.Controllers;

[Route("api/v1/register/drugs")]
public sealed class RegisterController(IRegisterService register) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] DateOnly? date, [FromQuery] int page = 1, [FromQuery] int limit = 50, CancellationToken cancellationToken = default) => FromResult(await register.GetDrugsAsync(date, page, limit, cancellationToken));

    [HttpGet("export")]
    public async Task<ActionResult> Export([FromQuery] DateOnly? date, [FromQuery] string? format, CancellationToken cancellationToken)
    {
        var result = await register.ExportDrugsAsync(date, format, cancellationToken);
        return result.Succeeded ? File(System.Text.Encoding.UTF8.GetBytes(result.Data ?? string.Empty), "text/csv", "drug-register.csv") : FromResult(result);
    }
}
