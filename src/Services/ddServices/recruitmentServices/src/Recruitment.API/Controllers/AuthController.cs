using Microsoft.AspNetCore.Mvc;
using Recruitment.API.Services;

namespace Recruitment.API.Controllers;

/// <summary>
/// Authentication controller for JWT token generation
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// Login endpoint to get JWT token
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT token</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // In production, validate against actual user store (Azure AD, database, etc.)
        // For demo purposes, we accept any valid request format
        if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Password))
        {
            _logger.LogWarning("Login attempt with invalid credentials for user: {UserId}", request.UserId);
            return Unauthorized(new { message = "Invalid username or password" });
        }

        try
        {
            // Generate token with user information
            var token = _tokenService.GenerateToken(
                request.UserId,
                request.UserName ?? request.UserId,
                request.Email ?? $"{request.UserId}@recruitment.local",
                request.Department
            );

            _logger.LogInformation("Successfully generated token for user: {UserId}", request.UserId);

            return Ok(new LoginResponse
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = 3600,
                UserId = request.UserId,
                UserName = request.UserName ?? request.UserId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating token for user: {UserId}", request.UserId);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Validate token endpoint
    /// </summary>
    /// <param name="request">Token to validate</param>
    /// <returns>Token validity</returns>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(TokenValidationResponse), StatusCodes.Status200OK)]
    public IActionResult ValidateToken([FromBody] TokenValidationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            return BadRequest(new { message = "Token is required" });
        }

        var isValid = _tokenService.ValidateToken(request.Token);
        return Ok(new TokenValidationResponse { IsValid = isValid });
    }
}

/// <summary>
/// Login request model
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// User ID or SPARSH ID
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// User password
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// User's full name (optional)
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// User's email address (optional)
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// User's department (optional)
    /// </summary>
    public string? Department { get; set; }
}

/// <summary>
/// Login response model
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// JWT access token
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Token type (Bearer)
    /// </summary>
    public string? TokenType { get; set; }

    /// <summary>
    /// Token expiration time in seconds
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// User ID
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// User name
    /// </summary>
    public string? UserName { get; set; }
}

/// <summary>
/// Token validation request
/// </summary>
public class TokenValidationRequest
{
    public string? Token { get; set; }
}

/// <summary>
/// Token validation response
/// </summary>
public class TokenValidationResponse
{
    public bool IsValid { get; set; }
}
