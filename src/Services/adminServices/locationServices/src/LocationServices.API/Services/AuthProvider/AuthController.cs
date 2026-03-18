using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocationServices.API.Services.AuthProvider;

/// <summary>
/// Authentication endpoint — issues JWT tokens.
/// Route: POST /api/auth/token
/// </summary>
[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public sealed class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IJwtService jwtService, ILogger<AuthController> logger)
    {
        _jwtService = jwtService;
        _logger     = logger;
    }

    /// <summary>Request a JWT token with username + password credentials</summary>
    [HttpPost("token")]
    [ProducesResponseType(typeof(AuthTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Token([FromBody] AuthRequest request)
    {
        // Demo validation — replace with real user store / identity provider
        if (!IsValidCredentials(request.Username, request.Password))
        {
            _logger.LogWarning("[Auth] Failed login attempt for {Username}", request.Username);
            return Unauthorized(new { message = "Invalid credentials." });
        }

        var roles  = GetUserRoles(request.Username);
        var result = _jwtService.GenerateToken(
            userId:   Guid.NewGuid().ToString(),
            username: request.Username,
            roles:    roles);

        _logger.LogInformation("[Auth] Token issued for {Username}", request.Username);
        return Ok(new AuthTokenResponse(result.Token, result.ExpiresAt, roles));
    }

    // ── private helpers ───────────────────────────────────────────────────────
    private static bool IsValidCredentials(string username, string password) =>
        // PRODUCTION: query user store; never store passwords in plain text
        (username == "admin" && password == "Admin@123") ||
        (username == "viewer" && password == "Viewer@123");

    private static string[] GetUserRoles(string username) =>
        username == "admin" ? ["Admin", "LocationManager"] : ["Viewer"];
}

// ── DTOs ──────────────────────────────────────────────────────────────────────
public sealed record AuthRequest(string Username, string Password);
public sealed record AuthTokenResponse(string Token, DateTime ExpiresAt, string[] Roles);
