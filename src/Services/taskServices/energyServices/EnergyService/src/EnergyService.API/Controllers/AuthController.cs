using EnergyService.API.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnergyService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenGenerator _tokenGenerator;

    public AuthController(JwtTokenGenerator tokenGenerator) => _tokenGenerator = tokenGenerator;

    [HttpPost("token")]
    [AllowAnonymous]
    public IActionResult GenerateToken([FromBody] LoginRequest request)
    {
        // In production, validate credentials against a user store
        if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            return BadRequest("Username and password are required.");

        var token = _tokenGenerator.GenerateToken(request.UserId.ToString(), request.UserName, request.Roles);
        return Ok(new { Token = token });
    }
}

public record LoginRequest(int UserId, string UserName, string Password, IEnumerable<string>? Roles = null);
