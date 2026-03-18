using AttendanceService.API.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(JwtTokenGenerator tokenGenerator) : ControllerBase
{
    public record LoginRequest(string Username, string Password);
    public record LoginResponse(string Token, DateTime Expires);

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // NOTE: Replace with real user validation against identity store
        if (request.Username == "admin" && request.Password == "Admin@123!")
        {
            var token = tokenGenerator.GenerateToken(1, request.Username, ["Admin", "Hr"]);
            return Ok(new LoginResponse(token, DateTime.UtcNow.AddHours(1)));
        }
        return Unauthorized(new { error = "Invalid credentials." });
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        return Ok(new { userId, username });
    }
}
