using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReferenceService.API.Auth;

/// <summary>
/// JWT token configuration.
/// </summary>
public class JwtConfiguration
{
    public string Key { get; set; } = "your-secret-key-must-be-at-least-32-characters-long";
    public string Issuer { get; set; } = "reference-service";
    public string Audience { get; set; } = "reference-service-api";
    public int ExpirationMinutes { get; set; } = 60;
}

/// <summary>
/// JWT token service.
/// </summary>
public interface IJwtTokenService
{
    string GenerateToken(string userId, string email, string[] roles);
    ClaimsPrincipal? ValidateToken(string token);
}

/// <summary>
/// JWT token service implementation.
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly JwtConfiguration _config;
    
    public JwtTokenService(JwtConfiguration config)
    {
        _config = config;
    }
    
    public string GenerateToken(string userId, string email, string[] roles)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email)
        };
        
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        var token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_config.ExpirationMinutes),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
            var handler = new JwtSecurityTokenHandler();
            
            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidIssuer = _config.Issuer,
                ValidateAudience = true,
                ValidAudience = _config.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            
            return principal;
        }
        catch
        {
            return null;
        }
    }
}
