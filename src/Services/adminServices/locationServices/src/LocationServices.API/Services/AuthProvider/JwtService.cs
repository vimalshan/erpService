using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LocationServices.API.Services.AuthProvider;

// ── OPTIONS ──────────────────────────────────────────────────────────────────
public sealed class JwtOptions
{
    public string Secret        { get; init; } = string.Empty;
    public string Issuer        { get; init; } = "LocationServices";
    public string Audience      { get; init; } = "LocationServices.Client";
    public int    ExpiryMinutes { get; init; } = 60;
}

// ── INTERFACE ────────────────────────────────────────────────────────────────
public interface IJwtService
{
    TokenResult GenerateToken(string userId, string username, IEnumerable<string> roles);
    ClaimsPrincipal? ValidateToken(string token);
}

public sealed record TokenResult(string Token, DateTime ExpiresAt);

// ── IMPLEMENTATION ────────────────────────────────────────────────────────────
public sealed class JwtService : IJwtService
{
    private readonly JwtOptions       _opts;
    private readonly SymmetricSecurityKey _key;

    public JwtService(IOptions<JwtOptions> options)
    {
        _opts = options.Value;
        _key  = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.Secret));
    }

    public TokenResult GenerateToken(string userId, string username, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,  userId),
            new(JwtRegisteredClaimNames.Name, username),
            new(JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var creds   = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_opts.ExpiryMinutes);

        var token = new JwtSecurityToken(
            issuer:             _opts.Issuer,
            audience:           _opts.Audience,
            claims:             claims,
            notBefore:          DateTime.UtcNow,
            expires:            expires,
            signingCredentials: creds);

        return new TokenResult(new JwtSecurityTokenHandler().WriteToken(token), expires);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var handler    = new JwtSecurityTokenHandler();
        var parameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey         = _key,
            ValidateIssuer           = true,
            ValidIssuer              = _opts.Issuer,
            ValidateAudience         = true,
            ValidAudience            = _opts.Audience,
            ValidateLifetime         = true,
            ClockSkew                = TimeSpan.Zero
        };

        try { return handler.ValidateToken(token, parameters, out _); }
        catch { return null; }
    }
}
