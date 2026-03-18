using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LovService.API.Controllers;

/// <summary>
/// Issues JWT tokens for development/testing. Replace with proper IdP in production.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IConfiguration config) : ControllerBase
{
    [HttpPost("token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetToken([FromBody] LoginRequest request)
    {
        // NOTE: Replace this stub with real user validation
        if (request.Username != "admin" || request.Password != "admin")
            return Unauthorized();

        var jwt = config.GetSection("Jwt");
        var keyBytes = Encoding.UTF8.GetBytes(jwt["SecretKey"]!);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, "User"),
        };

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

    public record LoginRequest(string Username, string Password);
}
