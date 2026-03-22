using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ScholarshipService.API.Controllers;

/// <summary>Issues JWT tokens for development/testing. In production, delegate to an Identity Provider.</summary>
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public sealed class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration) => _configuration = configuration;

    /// <summary>Get a JWT bearer token.</summary>
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Token([FromBody] TokenRequest request)
    {
        var validUser = _configuration["Auth:DemoUser"] ?? "admin";
        var validPass = _configuration["Auth:DemoPassword"] ?? "Admin@1234";

        if (request.Username != validUser || request.Password != validPass)
            return Unauthorized(new { message = "Invalid credentials." });

        var key = _configuration["Jwt:Key"] ?? "ScholarshipServiceDefaultSecretKey!!";
        var issuer = _configuration["Jwt:Issuer"] ?? "ScholarshipService";
        var audience = _configuration["Jwt:Audience"] ?? "ScholarshipServiceClient";
        var expiryMinutes = int.TryParse(_configuration["Jwt:ExpiryMinutes"], out var m) ? m : 480;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new TokenResponse(tokenString, "Bearer", expiryMinutes * 60));
    }
}

public record TokenRequest(string Username, string Password);
public record TokenResponse(string AccessToken, string TokenType, int ExpiresIn);
