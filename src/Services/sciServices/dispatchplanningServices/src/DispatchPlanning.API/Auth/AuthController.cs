using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DispatchPlanning.API.Auth;

public record LoginRequest(string Username, string Password, int UserId);

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _tokenService;

    public AuthController(IJwtTokenService tokenService) => _tokenService = tokenService;

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // In production: validate against user store/LDAP. 
        // Here we use a simple placeholder for demo purposes.
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return Unauthorized(new { message = "Invalid credentials." });

        var token = _tokenService.GenerateToken(request.UserId, request.Username, new[] { "User" });
        return Ok(new { token, expiresIn = 3600 });
    }
}
