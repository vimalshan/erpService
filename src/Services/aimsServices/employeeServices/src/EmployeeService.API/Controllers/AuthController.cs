using Microsoft.AspNetCore.Mvc;
using EmployeeService.API.Auth;

namespace EmployeeService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly JwtTokenService _tokenService;

    public AuthController(JwtTokenService tokenService) => _tokenService = tokenService;

    /// <summary>Issue a JWT for development/testing purposes.</summary>
    [HttpPost("token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetToken([FromBody] TokenRequest request)
    {
        // NOTE: In production, validate credentials against a user store / identity provider.
        // This endpoint is for development/testing only.
        if (string.IsNullOrWhiteSpace(request.Username))
            return BadRequest("Username is required.");

        var token = _tokenService.GenerateToken(request.UserId, request.Role ?? "Employee");
        return Ok(new { token, expiresAt = DateTime.UtcNow.AddHours(1) });
    }
}

public record TokenRequest(string Username, long UserId, string? Role);
