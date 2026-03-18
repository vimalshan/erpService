using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TrainingDevelopment.API.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration) => _configuration = configuration;

    /// <summary>
    /// Generate a JWT token for development/testing purposes.
    /// In production, integrate with your identity provider.
    /// </summary>
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GenerateToken([FromBody] LoginRequest request)
    {
        // Simple credential check — replace with real identity verification
        if (!IsValidUser(request.Username, request.Password))
            return Unauthorized(new { message = "Invalid credentials" });

        var token = CreateToken(request.Username, GetRoles(request.Username));
        return Ok(new TokenResponse(token, "Bearer", 3600));
    }

    private bool IsValidUser(string username, string password)
    {
        // Placeholder — integrate with LDAP/AD/IdentityServer in production
        return username == "admin" && password == "Admin@123" ||
               username == "hruser" && password == "HrUser@123";
    }

    private string[] GetRoles(string username) =>
        username == "admin" ? ["Admin", "HrUser"] : ["HrUser"];

    private string CreateToken(string username, string[] roles)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, username)
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var expiry = int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60");
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiry),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record LoginRequest(string Username, string Password);
public record TokenResponse(string Token, string TokenType, int ExpiresIn);
