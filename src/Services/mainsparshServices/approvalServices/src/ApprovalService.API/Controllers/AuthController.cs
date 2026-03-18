namespace ApprovalService.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using ApprovalService.Application.Interfaces;
using ApprovalService.Application.DTOs;
using MediatR;

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
    /// Login and get JWT token
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        try
        {
            // TODO: Integrate with actual authentication service
            // This is a placeholder implementation
            // In production, validate credentials against your authentication system

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // Placeholder validation (replace with actual authentication)
            if (request.Username != "admin" || request.Password != "admin123")
            {
                _logger.LogWarning("Failed login attempt for user {Username}", request.Username);
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // Generate token
            var userId = 1L; // Replace with actual user ID from auth system
            var token = _tokenService.GenerateToken(
                userId,
                request.Username,
                "Administrator");

            _logger.LogInformation("User {Username} logged in successfully", request.Username);

            return Ok(new TokenResponseDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = 86400 // 24 hours
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred" });
        }
    }

    /// <summary>
    /// Validate token
    /// </summary>
    [HttpGet("validate")]
    [ProducesResponseType(typeof(TokenValidationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult ValidateToken([FromHeader(Name = "Authorization")] string authorization)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(authorization) || !authorization.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Invalid authorization header" });
            }

            var token = authorization.Substring("Bearer ".Length);
            var isValid = _tokenService.ValidateToken(token);

            if (!isValid)
            {
                return Unauthorized(new { message = "Invalid or expired token" });
            }

            var userId = _tokenService.GetUserIdFromToken(token);

            return Ok(new TokenValidationResponseDto
            {
                IsValid = true,
                UserId = userId,
                Message = "Token is valid"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get current user info
    /// </summary>
    [HttpGet("me")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ProducesResponseType(typeof(CurrentUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetCurrentUser()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new { message = "User information not found in token" });
            }

            return Ok(new CurrentUserDto
            {
                UserId = long.Parse(userId),
                UserName = userName ?? "Unknown",
                Role = role ?? "User"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred" });
        }
    }
}

/// <summary>
/// Login request DTO
/// </summary>
public record LoginRequestDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

/// <summary>
/// Token response DTO
/// </summary>
public record TokenResponseDto
{
    public string AccessToken { get; set; } = "";
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; } = 86400;
}

/// <summary>
/// Token validation response DTO
/// </summary>
public record TokenValidationResponseDto
{
    public bool IsValid { get; set; }
    public long? UserId { get; set; }
    public string Message { get; set; } = "";
}

/// <summary>
/// Current user DTO
/// </summary>
public record CurrentUserDto
{
    public long UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Role { get; set; } = "";
}
