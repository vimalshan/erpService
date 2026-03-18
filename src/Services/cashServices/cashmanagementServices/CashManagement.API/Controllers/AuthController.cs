using Microsoft.AspNetCore.Mvc;
using CashManagement.API.Auth;

namespace CashManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(JwtTokenService tokenService, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetToken([FromBody] LoginRequest request)
    {
        // NOTE: Replace with real user credential validation from a user store.
        // This is a placeholder implementation for development.
        if (request.Username == "admin" && request.Password == "admin123")
        {
            var token = _tokenService.GenerateToken(1, request.Username, new[] { "Admin", "CashManager" });
            _logger.LogInformation("Token issued for user: {Username}", request.Username);
            return Ok(new TokenResponse(token, DateTime.UtcNow.AddHours(1)));
        }

        _logger.LogWarning("Failed login attempt for user: {Username}", request.Username);
        return Unauthorized(new { message = "Invalid credentials." });
    }
}

public record LoginRequest(string Username, string Password);
public record TokenResponse(string AccessToken, DateTime ExpiresAt);
