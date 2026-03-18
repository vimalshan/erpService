using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TdsService.API.Controllers;

/// <summary>Issues JWT tokens. This is a simplified token endpoint for development.
/// In production, delegate to an Identity Provider (Keycloak, Azure AD B2C, etc.).</summary>
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public sealed class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration) => _configuration = configuration;

    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Token([FromBody] TokenRequest request)
    {
        // NOTE: Demo only. Replace with real identity store and password hashing.
        var validUser = _configuration["Auth:DemoUser"] ?? "admin";
        var validPass = _configuration["Auth:DemoPassword"] ?? "Admin@1234";

        if (request.Username != validUser || request.Password != validPass)
            return Unauthorized(new { message = "Invalid credentials." });

        var token = JwtHelper.GenerateToken(
            _configuration["Jwt:Key"]!,
            _configuration["Jwt:Issuer"]!,
            _configuration["Jwt:Audience"]!,
            request.Username,
            TimeSpan.FromHours(8));

        return Ok(new TokenResponse(token, "Bearer", 3600 * 8));
    }
}

public sealed record TokenRequest(string Username, string Password);
public sealed record TokenResponse(string AccessToken, string TokenType, int ExpiresIn);
