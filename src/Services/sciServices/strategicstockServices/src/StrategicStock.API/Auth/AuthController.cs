using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace StrategicStock.API.Auth;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IOptions<JwtSettings> jwtSettings) : ControllerBase
{
    /// <summary>
    /// Generate a JWT token for testing. In production, integrate with your identity provider.
    /// </summary>
    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] LoginRequest request)
    {
        // Simplified authentication — replace with a real identity provider in production
        if (string.IsNullOrWhiteSpace(request.Username))
            return BadRequest("Username is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Password is required.");

        // TODO: Replace with real user/password validation (e.g., ASP.NET Identity, LDAP, external IdP)
        // This is a demo-only check — do NOT ship hardcoded credentials
        if (request.Username != "admin" || request.Password != "test")
            return Unauthorized("Invalid credentials.");

        var settings = jwtSettings.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, "User"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(settings.ExpiryMinutes),
            signingCredentials: credentials);

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}

public sealed record LoginRequest(string Username, string Password);
