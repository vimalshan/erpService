using DealTicketing.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DealTicketing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController(IJwtTokenService jwtService) : ControllerBase
{
    /// <summary>Authenticate and obtain a JWT token.</summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // SIMPLIFIED: In production, validate against identity store / AD.
        if (request.Username == "admin" && request.Password == "Admin@123")
        {
            var token = jwtService.GenerateToken(request.Username, ["Admin"]);
            return Ok(new { Token = token, ExpiresIn = 3600 });
        }

        if (request.Username == "approver" && request.Password == "Approver@123")
        {
            var token = jwtService.GenerateToken(request.Username, ["DealApprover"]);
            return Ok(new { Token = token, ExpiresIn = 3600 });
        }

        return Unauthorized(new { Message = "Invalid credentials." });
    }
}

public record LoginRequest(string Username, string Password);
