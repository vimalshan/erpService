using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EmailNotification.API.Controllers;

/// <summary>
/// Authentication controller for JWT token generation
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Login and generate JWT token
    /// </summary>
    /// <param name="request">User credentials</param>
    /// <returns>JWT token response</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // TODO: Validate credentials against user database
        // This is a placeholder implementation
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return Unauthorized(new ErrorResponse 
            { 
                Message = "Invalid username or password",
                Timestamp = DateTime.UtcNow
            });

        try
        {
            var token = GenerateJwtToken(request.Username);
            _logger.LogInformation("User {Username} logged in successfully at {Time}", 
                request.Username, DateTime.UtcNow);
            
            return Ok(new AuthResponse
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = GetTokenExpirationMinutes() * 60, // Convert to seconds
                IssuedAt = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating JWT token for user {Username}", request.Username);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new ErrorResponse 
                { 
                    Message = "An error occurred while generating the token",
                    Timestamp = DateTime.UtcNow
                });
        }
    }

    /// <summary>
    /// Refresh JWT token
    /// </summary>
    /// <param name="request">Refresh token request</param>
    /// <returns>New JWT token</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // TODO: Validate refresh token from database
            // For now, this is a placeholder
            var principal = GetPrincipalFromExpiredToken(request.RefreshToken);
            if (principal == null)
                return Unauthorized(new ErrorResponse 
                { 
                    Message = "Invalid or expired refresh token",
                    Timestamp = DateTime.UtcNow
                });

            var username = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "User";
            var newAccessToken = GenerateJwtToken(username);

            return Ok(new AuthResponse
            {
                AccessToken = newAccessToken,
                TokenType = "Bearer",
                ExpiresIn = GetTokenExpirationMinutes() * 60,
                IssuedAt = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing JWT token");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponse 
                { 
                    Message = "An error occurred while refreshing the token",
                    Timestamp = DateTime.UtcNow
                });
        }
    }

    /// <summary>
    /// Generates a JWT token for the specified username
    /// </summary>
    private string GenerateJwtToken(string username)
    {
        var secret = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT:Secret not configured");
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = GetTokenExpirationMinutes();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(ClaimTypes.Name, username),
            new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new Claim(ClaimTypes.Role, "User") // Default role, customize as needed
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "EmailNotificationAPI",
            audience: _configuration["Jwt:Audience"] ?? "emailnotification-api",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiration),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Gets the principal from an expired token (for token refresh)
    /// </summary>
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ?? "default-secret")),
            ValidateLifetime = false // Allow expired tokens for refresh
        };

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the token expiration time in minutes from configuration
    /// </summary>
    private int GetTokenExpirationMinutes()
    {
        var configValue = _configuration["Jwt:ExpirationMinutes"];
        return int.TryParse(configValue, out var minutes) ? minutes : 60;
    }
}

/// <summary>
/// Login request model
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Username for authentication
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Password for authentication
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Refresh token request model
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// The refresh token to exchange for a new access token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// Authentication response model
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// JWT access token
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Token type (Bearer)
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Token expiration in seconds
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// When the token was issued
    /// </summary>
    public DateTime IssuedAt { get; set; }
}

/// <summary>
/// Error response model
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Error message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of the error
    /// </summary>
    public DateTime Timestamp { get; set; }
}
