using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly Auth.JwtTokenService _tokenService;

    public AuthController(Auth.JwtTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <summary>
    /// Generate a JWT token for testing
    /// </summary>
    [HttpPost("token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult GenerateToken([FromBody] LoginRequest request)
    {
        // In production, validate against your user store
        if (string.IsNullOrWhiteSpace(request.UserName))
            return BadRequest("Username is required");

        var token = _tokenService.GenerateToken(
            userId: request.UserId ?? "1",
            userName: request.UserName,
            roles: request.Roles ?? ["User"]);

        return Ok(new { Token = token, ExpiresIn = "8h" });
    }
}

public record LoginRequest
{
    public string? UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string[]? Roles { get; init; }
}
