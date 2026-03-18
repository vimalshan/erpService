using BusServices.API.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public sealed class AuthController : ControllerBase
{
    private readonly IJwtTokenGenerator _jwtGenerator;

    public AuthController(IJwtTokenGenerator jwtGenerator) => _jwtGenerator = jwtGenerator;

    /// <summary>Authenticate and receive a JWT token.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), 200)]
    [ProducesResponseType(401)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Demo credential validation — replace with real user service in production
        if (request.Username != "admin" || request.Password != "Admin@1234")
            return Unauthorized(new { message = "Invalid credentials." });

        var token = _jwtGenerator.GenerateToken(1, request.Username, new[] { "Admin", "BusManager" });
        return Ok(new LoginResponse(token, DateTime.UtcNow.AddHours(1)));
    }
}

public record LoginRequest(string Username, string Password);
public record LoginResponse(string Token, DateTime ExpiresAt);
