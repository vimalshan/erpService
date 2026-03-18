using ExitManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExitManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtService;

    public AuthController(IJwtTokenService jwtService) => _jwtService = jwtService;

    /// <summary>Generates a JWT token for the given credentials.</summary>
    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] LoginRequest request)
    {
        // NOTE: In production, validate credentials against an identity store.
        // This is a demonstration endpoint.
        if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Email))
            return BadRequest("UserId and Email are required.");

        var token = _jwtService.GenerateToken(request.UserId, request.Email, request.Roles ?? new[] { "Employee" });
        return Ok(new { token, expiresIn = "60 minutes" });
    }
}

public record LoginRequest(string UserId, string Email, string[]? Roles);
