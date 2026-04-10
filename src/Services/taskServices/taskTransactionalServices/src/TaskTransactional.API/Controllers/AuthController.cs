using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TaskTransactional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    public record LoginRequest(string Username, string Password);
    public record TokenResponse(string Token, DateTime Expiry);

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Demo authentication - replace with real identity provider in production
        if (request.Username != "admin" || request.Password != "admin123")
            return Unauthorized(new { message = "Invalid credentials" });

        var token = GenerateJwtToken(request.Username);
        return Ok(token);
    }

    private TokenResponse GenerateJwtToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            configuration["Jwt:Key"] ?? "TaskTransactional-Super-Secret-Key-For-JWT-Authentication-2024!"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddHours(8);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, username)
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"] ?? "TaskTransactional",
            audience: configuration["Jwt:Audience"] ?? "TaskTransactional",
            claims: claims,
            expires: expiry,
            signingCredentials: credentials);

        return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token), expiry);
    }
}
