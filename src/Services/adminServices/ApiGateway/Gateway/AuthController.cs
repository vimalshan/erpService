using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiGateway.Gateway;

[ApiController]
[Route("api/gateway/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Generate JWT token for gateway authentication
    /// </summary>
    [HttpPost("token")]
    [AllowAnonymous]
    public IActionResult GenerateToken([FromBody] TokenRequest request)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "https://erpmicroservice.com";
        var audience = jwtSettings["Audience"] ?? "erp-api-users";
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, request.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("name", request.Username),
            new("role", request.Role ?? "user"),
            new("scope", "finyear-api"),
            new("scope", "location-api"),
            new("scope", "vendor-api"),
            new("scope", "scholarship-api"),
            new("scope", "stationery-api"),
            new("scope", "tds-api"),
            new("scope", "lov-api"),
            new("scope", "shared-api"),
            new("scope", "transaction-api"),
            new("permission", "read"),
            new("permission", "write")
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiresIn = expirationMinutes * 60,
            tokenType = "Bearer"
        });
    }
}

public class TokenRequest
{
    public string Username { get; set; } = "admin";
    public string? Role { get; set; } = "admin";
}
