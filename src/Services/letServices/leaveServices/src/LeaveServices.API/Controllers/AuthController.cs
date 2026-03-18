using LeaveServices.API.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LeaveServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtService;
    public AuthController(IJwtTokenService jwtService) => _jwtService = jwtService;

    /// <summary>Authenticate and receive a JWT token.</summary>
    [HttpPost("token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Token([FromBody] LoginRequest request)
    {
        // TODO: Replace with real user store / LDAP validation
        if (request.Username == "admin" && request.Password == "admin123")
        {
            var token = _jwtService.GenerateToken(request.Username, $"{request.Username}@erp.local", ["Admin", "Manager", "HR"]);
            return Ok(new { token });
        }

        if (request.Username == "user" && request.Password == "user123")
        {
            var token = _jwtService.GenerateToken(request.Username, $"{request.Username}@erp.local", ["Employee"]);
            return Ok(new { token });
        }

        return Unauthorized(new { message = "Invalid credentials." });
    }
}

public record LoginRequest(string Username, string Password);
