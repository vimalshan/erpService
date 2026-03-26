using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration) => _configuration = configuration;

    [HttpPost("token")]
    public IActionResult GetToken([FromBody] LoginRequest request)
    {
        // Gateway-level auth: issue a token accepted by all downstream services
        if (request.Username != "admin" || request.Password != "admin123")
            return Unauthorized(new { message = "Invalid credentials." });

        var key = _configuration["Jwt:Key"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var expires = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim("gateway", "true")
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expires),
            signingCredentials: credentials);

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}

public record LoginRequest(string Username, string Password);
