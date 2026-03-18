using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace TaxService.API.Controllers;

/// <summary>
/// Authentication controller for JWT token generation
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Generate a JWT token for API authentication
    /// </summary>
    /// <param name="request">Token request with user details</param>
    /// <returns>JWT token for use in Authorization header</returns>
    [HttpPost("token")]
    public ActionResult<TokenResponse> GenerateToken([FromBody] TokenRequest request)
    {
        if (string.IsNullOrEmpty(request?.UserId))
            return BadRequest(new { message = "UserId is required" });

        try
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.ASCII.GetBytes(
                jwtSettings["SecretKey"] ?? "your-secret-key-change-in-production");
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("sub", request.UserId),
                    new Claim(ClaimTypes.NameIdentifier, request.UserId),
                    new Claim(ClaimTypes.Name, request.UserName ?? request.UserId),
                    new Claim("role", request.Role ?? "User")
                }),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new TokenResponse
            {
                Token = tokenString,
                ExpiresIn = expirationMinutes * 60, // seconds
                TokenType = "Bearer",
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error generating token", error = ex.Message });
        }
    }

    /// <summary>
    /// Get a sample token for testing (hardcoded credentials)
    /// </summary>
    [HttpGet("sample-token")]
    public ActionResult<TokenResponse> GetSampleToken()
    {
        return GenerateToken(new TokenRequest
        {
            UserId = "user123",
            UserName = "Test User",
            Role = "Admin"
        });
    }
}

/// <summary>
/// Request model for token generation
/// </summary>
public class TokenRequest
{
    /// <summary>
    /// Unique user identifier
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// User display name (optional)
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// User role (optional, defaults to "User")
    /// </summary>
    public string? Role { get; set; }
}

/// <summary>
/// Response model for token generation
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// The JWT bearer token
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Token type (always "Bearer")
    /// </summary>
    public string? TokenType { get; set; }

    /// <summary>
    /// Token expiration time in seconds
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Exact expiration datetime
    /// </summary>
    public DateTime ExpiresAt { get; set; }
}
