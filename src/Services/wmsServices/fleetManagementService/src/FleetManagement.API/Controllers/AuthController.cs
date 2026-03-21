using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FleetManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    public record LoginRequest(string Username, string Password);
    public record TokenResponse(string Token, DateTime Expiry);

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // In production, validate against a real user store
        if (request.Username != "admin" || request.Password != "admin")
            return Unauthorized(new { Message = "Invalid credentials" });

        var token = GenerateToken(request.Username);
        return Ok(token);
    }

    private TokenResponse GenerateToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(
            int.TryParse(configuration["Jwt:ExpiryMinutes"], out var m) ? m : 60);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds
        );

        return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token), expiry);
    }
}
