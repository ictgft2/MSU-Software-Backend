using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gilead.API.Auth;
using Gilead.Application;
using Gilead.Application.DTOs;
using Gilead.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gilead.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController(IStaffService staff, IOptions<JwtOptions> options) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await staff.AuthenticateAsync(request, cancellationToken);
        if (!result.Succeeded || result.Data is null)
            return Unauthorized(result);

        var jwt = options.Value;
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(jwt.ExpirationMinutes);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, result.Data.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, result.Data.Email),
            new Claim(ClaimTypes.Name, result.Data.FullName),
            new Claim(ClaimTypes.Role, result.Data.Role.ToString())
        };
        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(jwt.Issuer, jwt.Audience, claims, expires: expiresAt.UtcDateTime, signingCredentials: credentials);
        return Ok(ServiceResult<LoginResponse>.Ok(new LoginResponse(new JwtSecurityTokenHandler().WriteToken(token), expiresAt, result.Data)));
    }
}