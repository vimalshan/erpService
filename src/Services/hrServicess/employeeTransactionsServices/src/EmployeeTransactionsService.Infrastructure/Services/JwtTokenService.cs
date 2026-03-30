using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeTransactionsService.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeTransactionsService.Infrastructure.Services;

public sealed class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    public string GenerateToken(string username, string[] roles)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var secret = jwtSection["Secret"] ?? throw new InvalidOperationException("JWT secret is missing.");
        var issuer = jwtSection["Issuer"] ?? "EmployeeTransactionsService";
        var audience = jwtSection["Audience"] ?? "EmployeeTransactionsClients";
        var expiryHours = int.TryParse(jwtSection["ExpiryHours"], out var parsedHours) ? parsedHours : 8;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expiryHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}