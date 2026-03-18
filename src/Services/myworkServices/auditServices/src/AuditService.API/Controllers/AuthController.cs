using AuditService.Application.DTOs;
using AuditService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IJwtService jwtService, ILogger<AuthController> logger)
    {
        _jwtService = jwtService;
        _logger = logger;
    }

    /// <summary>Authenticates a user and returns a JWT token.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] AuthRequest request)
    {
        // In production, validate against AuditUserMaster. Placeholder check here.
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return Unauthorized(new { message = "Invalid credentials." });

        // Demo: accept any non-empty credentials; replace with real user lookup.
        var role = request.Username.StartsWith("admin", StringComparison.OrdinalIgnoreCase) ? "Admin" : "Auditor";
        var response = _jwtService.GenerateToken(request.Username, role, 1);

        _logger.LogInformation("User '{Username}' authenticated successfully.", request.Username);
        return Ok(response);
    }

    /// <summary>Returns the current user's claims.</summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Me()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(claims);
    }
}
