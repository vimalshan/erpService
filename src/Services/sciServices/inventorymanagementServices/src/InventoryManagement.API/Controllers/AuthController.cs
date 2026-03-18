using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace InventoryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config) => _config = config;

    /// <summary>Obtain a JWT token for testing.</summary>
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(401)]
    public IActionResult Token([FromBody] LoginRequest request)
    {
        // Demo-only credential check; replace with actual identity provider in production.
        var adminUser = _config["Auth:AdminUser"] ?? "admin";
        var adminPass = _config["Auth:AdminPassword"] ?? "Admin@1234";

        if (request.Username != adminUser || request.Password != adminPass)
            return Unauthorized(new { message = "Invalid credentials." });

        var jwt = GenerateToken(request.Username);
        return Ok(new TokenResponse(jwt, 3600));
    }

    private string GenerateToken(string username)
    {
        var jwtSection = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Inventory"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record LoginRequest(string Username, string Password);
public record TokenResponse(string Token, int ExpiresIn);
