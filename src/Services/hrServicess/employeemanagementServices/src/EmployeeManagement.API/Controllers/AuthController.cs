using EmployeeManagement.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtService;

    public AuthController(IJwtTokenService jwtService) => _jwtService = jwtService;

    public sealed record LoginRequest(string Username, string Password);
    public sealed record LoginResponse(string Token, string Username);

    /// <summary>Authenticate and receive a JWT token.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // In production, validate against your user store / LDAP
        if (request.Username == "admin" && request.Password == "Admin@123!")
        {
            var token = _jwtService.GenerateToken(1, request.Username, ["Admin", "HRManager"]);
            return Ok(new LoginResponse(token, request.Username));
        }

        return Unauthorized();
    }
}
