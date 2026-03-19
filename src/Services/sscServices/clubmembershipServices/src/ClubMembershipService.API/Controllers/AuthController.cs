using Microsoft.AspNetCore.Mvc;
using ClubMembershipService.API.Services;

namespace ClubMembershipService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtService;

    public AuthController(JwtTokenService jwtService) => _jwtService = jwtService;

    [HttpPost("token")]
    public IActionResult GetToken([FromBody] LoginRequest request)
    {
        // Demo validation — replace with real user lookup
        if (request.Username == "admin" && request.Password == "admin123")
        {
            var token = _jwtService.GenerateToken(1, request.Username, ["Admin", "User"]);
            return Ok(new { token });
        }
        return Unauthorized();
    }
}

public record LoginRequest(string Username, string Password);
