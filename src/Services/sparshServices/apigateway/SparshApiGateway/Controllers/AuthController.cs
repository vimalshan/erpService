using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace SparshApiGateway.Controllers;

/// <summary>
/// Gateway-level authentication controller.
/// Issues JWT tokens that are accepted by all downstream services.
/// </summary>
[ApiController]
[Route("api/gateway/[controller]")]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    public record LoginRequest(string Username, string Password);
    public record TokenResponse(string Token, DateTime Expiration, string[] Roles);

    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // In production, integrate with an identity provider (e.g. Azure AD, IdentityServer).
        if (request.Username != "admin" || request.Password != "admin123")
            return Unauthorized(new { error = "Invalid credentials." });

        var roles = new[] { "Admin", "Approver", "User" };
        var token = GenerateJwtToken(request.Username, roles);
        return Ok(token);
    }

    private TokenResponse GenerateJwtToken(string username, string[] roles)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            jwtSettings["SecretKey"] ?? "SparshApiGateway_SuperSecret_Key_2026_Minimum32Chars!!"));

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, username),
            new("gateway", "SparshApiGateway")
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var expirationHours = int.Parse(jwtSettings["ExpirationInHours"] ?? "4");
        var expiration = DateTime.UtcNow.AddHours(expirationHours);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"] ?? "SparshApiGateway",
            audience: jwtSettings["Audience"] ?? "SparshServices",
            claims: claims,
            expires: expiration,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token), expiration, roles);
    }
}
