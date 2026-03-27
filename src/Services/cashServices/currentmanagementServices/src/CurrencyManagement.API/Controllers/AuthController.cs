using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CurrencyManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("token")]
    public IActionResult GetToken([FromBody] LoginRequest request)
    {
        if (request.Username == "admin" && request.Password == "admin123")
        {
            var key = _configuration["Jwt:Key"] ?? "your-super-secret-key-that-is-at-least-32-characters-long!!!";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("sub", "1")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "CurrencyManagement",
                audience: _configuration["Jwt:Audience"] ?? "CurrencyManagementAPI",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials);

            return Ok(new { accessToken = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        return Unauthorized(new { message = "Invalid credentials" });
    }
}

public record LoginRequest(string Username, string Password);
