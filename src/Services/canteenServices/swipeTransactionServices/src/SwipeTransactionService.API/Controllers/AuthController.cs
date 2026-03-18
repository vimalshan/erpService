using Microsoft.AspNetCore.Mvc;
using SwipeTransactionService.Infrastructure.Auth;

namespace SwipeTransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(JwtTokenService jwtTokenService)
        => _jwtTokenService = jwtTokenService;

    public sealed record LoginRequest(string Username, string Password);
    public sealed record TokenResponse(string AccessToken, DateTime ExpiresAt);

    /// <summary>Authenticates and returns a JWT access token.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // NOTE: Replace with real user validation against your identity store.
        // This is a placeholder for demonstration purposes only.
        if (request.Username != "admin" || request.Password != "P@ssw0rd!")
            return Unauthorized(new { Message = "Invalid credentials." });

        var token = _jwtTokenService.GenerateToken(
            userId: "1",
            username: request.Username,
            roles: new[] { "Admin", "CanteenManager" });

        return Ok(new TokenResponse(token, DateTime.UtcNow.AddHours(1)));
    }
}
