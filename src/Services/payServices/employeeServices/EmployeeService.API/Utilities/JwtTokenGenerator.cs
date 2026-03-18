using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeService.API.Utilities;

/// <summary>
/// JWT token generator for testing and token creation
/// </summary>
public class JwtTokenGenerator
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationMinutes;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        _secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        _issuer = jwtSettings["Issuer"] ?? "EmployeeService";
        _audience = jwtSettings["Audience"] ?? "EmployeeServiceApi";
        _expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");
    }

    /// <summary>
    /// Generate JWT token for a user
    /// </summary>
    public string GenerateToken(string userId, string userName, string role, string email)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Email, email),
            new Claim("role", role),
            new Claim("userId", userId)
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Generate JWT token with multiple roles
    /// </summary>
    public string GenerateToken(string userId, string userName, string[] roles, string email)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claimsList = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Email, email),
            new Claim("userId", userId)
        };

        // Add all roles as separate claims
        foreach (var role in roles)
        {
            claimsList.Add(new Claim("role", role));
        }

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claimsList,
            expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Generate test tokens for different roles
    /// </summary>
    public Dictionary<string, string> GenerateTestTokens()
    {
        return new Dictionary<string, string>
        {
            ["Admin"] = GenerateToken("admin-001", "Admin User", "Admin", "admin@example.com"),
            ["Manager"] = GenerateToken("manager-001", "Manager User", "Manager", "manager@example.com"),
            ["Employee"] = GenerateToken("emp-001", "Employee User", "Employee", "employee@example.com"),
            ["ManagerAndAdmin"] = GenerateToken("multi-001", "Multi Role User", new[] { "Manager", "Admin" }, "multirole@example.com")
        };
    }
}
