using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BatchService.API.Auth;

public static class JwtTokenHelper
{
    public static string GenerateToken(string userId, string role, IConfiguration config)
    {
        var key     = config["Jwt:Key"]    ?? throw new InvalidOperationException("Jwt:Key not configured.");
        var issuer  = config["Jwt:Issuer"] ?? "BatchService";
        var audience = config["Jwt:Audience"] ?? "BatchServiceClients";
        var expMins  = int.TryParse(config["Jwt:ExpiryMinutes"], out var m) ? m : 60;

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityKey  = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials  = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token        = new JwtSecurityToken(issuer, audience, claims,
                               expires: DateTime.UtcNow.AddMinutes(expMins),
                               signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
