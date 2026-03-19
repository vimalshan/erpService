using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MamAllocationService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    public record LoginRequest(string Username, string Password);
    public record LoginResponse(string Token, DateTime Expiration);

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // In production, validate against a user store
        if (request.Username != "admin" || request.Password != "admin")
            return Unauthorized(new { message = "Invalid credentials" });

        var key = configuration["Jwt:Key"] ?? "MamAllocationServiceSuperSecretKey2026!@#$%^&*()";
        var issuer = configuration["Jwt:Issuer"] ?? "MamAllocationService";
        var audience = configuration["Jwt:Audience"] ?? "MamAllocationService";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expiration = DateTime.UtcNow.AddHours(2);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return Ok(new LoginResponse(new JwtSecurityTokenHandler().WriteToken(token), expiration));
    }
}
