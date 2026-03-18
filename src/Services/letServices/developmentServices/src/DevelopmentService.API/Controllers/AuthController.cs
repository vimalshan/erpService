using Microsoft.AspNetCore.Mvc;

namespace DevelopmentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(IJwtTokenService jwtTokenService)
        => _jwtTokenService = jwtTokenService;

    public record LoginRequest(string UserId, string Password);
    public record TokenResponse(string Token, DateTime Expires);

    /// <summary>Issues a JWT token. In production, validate credentials against your identity provider.</summary>
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Token([FromBody] LoginRequest request)
    {
        // DEMO ONLY: Replace this with real credential validation (e.g. AD, Identity).
        if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Password))
            return Unauthorized();

        var token   = _jwtTokenService.GenerateToken(request.UserId, ["User"]);
        var expires = DateTime.UtcNow.AddMinutes(60);
        return Ok(new TokenResponse(token, expires));
    }
}
