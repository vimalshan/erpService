using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SecurityService.Application.DTOs;
using SecurityService.Application.Interfaces;
using SecurityService.Application.Queries;
using MediatR;

namespace SecurityService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IMediator _mediator;

    public AuthController(IConfiguration config, IMediator mediator)
    {
        _config = config;
        _mediator = mediator;
    }

    /// <summary>
    /// Authenticate and receive a JWT token.
    /// NOTE: In production integrate with an Identity Provider; this is a demonstration endpoint.
    /// </summary>
    [HttpPost("token")]
    public async Task<ActionResult<AuthTokenDto>> Token([FromBody] LoginDto login, CancellationToken ct = default)
    {
        // Validate user exists (password validation is outside the scope of this demo;
        // in production integrate with ASP.NET Core Identity or an external IdP).
        var user = await _mediator.Send(new GetUserByCodeQuery(login.UserCode), ct);
        if (user is null || !user.IsActive)
            return Unauthorized("Invalid credentials.");

        var jwtSection = _config.GetSection("JwtSettings");
        var secret = jwtSection["Secret"] ?? throw new InvalidOperationException("JWT secret not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var expiryMinutes = int.TryParse(jwtSection["ExpiryMinutes"], out var m) ? m : 60;

        var userRoles = await _mediator.Send(new GetUserRolesQuery(user.UserId), ct);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserCode),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(userRoles.Select(r => new Claim(ClaimTypes.Role, r.RoleName ?? r.RoleId.ToString())));

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return Ok(new AuthTokenDto(new JwtSecurityTokenHandler().WriteToken(token), "Bearer", expiryMinutes * 60));
    }
}
