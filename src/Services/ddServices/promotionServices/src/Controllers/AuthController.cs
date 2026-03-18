using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromotionService.Infrastructure.Auth;

namespace PromotionService.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[AllowAnonymous]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<AuthController> _logger;

    // Hardcoded dev/test users — replace with a real user store or delegate to a central auth service in production.
    private static readonly Dictionary<string, (string Password, string Role)> DevUsers = new(StringComparer.OrdinalIgnoreCase)
    {
        ["admin"]  = ("Admin@123",  "Admin"),
        ["hr"]     = ("Hr@123",     "HR"),
        ["viewer"] = ("View@123",   "Viewer")
    };

    public AuthController(IJwtTokenService jwtTokenService, ILogger<AuthController> logger)
    {
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    /// <summary>Obtain a JWT bearer token (dev/test only — replace with central auth in production)</summary>
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Token([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Username and password are required.");

        if (!DevUsers.TryGetValue(request.Username, out var user) || user.Password != request.Password)
        {
            _logger.LogWarning("Failed login attempt for user {Username}", request.Username);
            return Unauthorized("Invalid credentials.");
        }

        var token = _jwtTokenService.GenerateToken(
            userId: request.Username.ToLowerInvariant(),
            username: request.Username,
            role: user.Role);

        _logger.LogInformation("Token issued for user {Username} with role {Role}", request.Username, user.Role);

        return Ok(new TokenResponse
        {
            AccessToken = token,
            TokenType = "Bearer",
            Role = user.Role,
            ExpiresInSeconds = 3600
        });
    }

    /// <summary>Validate a JWT token and return its claims</summary>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(ValidateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Validate([FromBody] ValidateRequest request)
    {
        var principal = _jwtTokenService.ValidateToken(request.Token);
        if (principal == null)
            return Unauthorized("Token is invalid or expired.");

        return Ok(new ValidateResponse
        {
            Valid = true,
            Username = principal.Identity?.Name,
            Role = principal.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
        });
    }
}

// ── Request / Response models ──────────────────────────────────────────────
public record LoginRequest(string Username, string Password);
public record ValidateRequest(string Token);

public record TokenResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string TokenType { get; init; } = "Bearer";
    public string Role { get; init; } = string.Empty;
    public int ExpiresInSeconds { get; init; }
}

public record ValidateResponse
{
    public bool Valid { get; init; }
    public string? Username { get; init; }
    public string? Role { get; init; }
}
