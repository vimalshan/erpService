using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace PayTransactionalService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AuthController(IConfiguration configuration) => _configuration = configuration;

    [HttpPost("token")]
    public ActionResult<TokenResponse> GenerateToken([FromBody] TokenRequest request)
    {
        if (string.IsNullOrEmpty(request?.UserId))
            return BadRequest(new { message = "UserId is required" });

        try
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "your-secret-key-change-in-production");
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
                ExpiresIn = expirationMinutes * 60,
                TokenType = "Bearer",
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error generating token", error = ex.Message });
        }
    }

    [HttpGet("sample-token")]
    public ActionResult<TokenResponse> GetSampleToken()
    {
        return GenerateToken(new TokenRequest { UserId = "user123", UserName = "Test User", Role = "Admin" });
    }
}

public class TokenRequest
{
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Role { get; set; }
}

public class TokenResponse
{
    public string? Token { get; set; }
    public string? TokenType { get; set; }
    public int ExpiresIn { get; set; }
    public DateTime ExpiresAt { get; set; }
}
