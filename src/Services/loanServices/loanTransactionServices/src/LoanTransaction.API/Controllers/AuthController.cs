using LoanTransaction.API.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace LoanTransaction.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtService;

    public AuthController(JwtTokenService jwtService) => _jwtService = jwtService;

    /// <summary>Login and receive a JWT token.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Simple demo auth — replace with real user store in production
        if (string.IsNullOrWhiteSpace(request.UserId))
            return Unauthorized(new { message = "Invalid credentials." });

        var token = _jwtService.GenerateToken(request.UserId, request.Role ?? "User");
        return Ok(new
        {
            token,
            expires = DateTime.UtcNow.AddHours(1)
        });
    }
}

public record LoginRequest(string UserId, string? Role);
