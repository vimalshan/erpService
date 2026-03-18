using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TimeAttendance.API.Controllers;

public record LoginRequest(string Username, string Password);
public record TokenResponse(string AccessToken, DateTime ExpiresAt);

[ApiController]
[Route("api/v1/[controller]")]
[AllowAnonymous]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    // NOTE: In production, validate credentials against a real identity store.
    // This is a demonstration endpoint only.
    private static readonly Dictionary<string, (string PasswordHash, string Role)> _users = new(StringComparer.OrdinalIgnoreCase)
    {
        ["admin"] = ("admin123", "Admin"),
        ["manager"] = ("manager123", "Manager"),
        ["reader"] = ("reader123", "Reader")
    };

    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!_users.TryGetValue(request.Username, out var userInfo) ||
            userInfo.PasswordHash != request.Password)
            return Unauthorized(new { message = "Invalid credentials." });

        var token = GenerateJwtToken(request.Username, userInfo.Role);
        return Ok(token);
    }

    private TokenResponse GenerateJwtToken(string username, string role)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var secret = jwtSection["Secret"]!;
        var issuer = jwtSection["Issuer"]!;
        var audience = jwtSection["Audience"]!;
        var expiryMinutes = int.Parse(jwtSection["ExpiryMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(issuer, audience, claims,
            expires: expiry, signingCredentials: creds);

        return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token), expiry);
    }
}
