using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace SparshTransactional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    public record LoginRequest(string Username, string Password);
    public record TokenResponse(string Token, DateTime Expiration);

    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Username != "admin" || request.Password != "admin123")
            return Unauthorized(new { error = "Invalid credentials." });

        var token = GenerateJwtToken(request.Username, ["Admin", "Approver"]);
        return Ok(token);
    }

    private TokenResponse GenerateJwtToken(string username, string[] roles)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            jwtSettings["SecretKey"] ?? "SparshTransactional_SuperSecret_Key_2026_Minimum32Chars!"));

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, username)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var expiration = DateTime.UtcNow.AddHours(2);
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"] ?? "SparshTransactional.API",
            audience: jwtSettings["Audience"] ?? "SparshTransactional.Client",
            claims: claims,
            expires: expiration,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token), expiration);
    }
}
