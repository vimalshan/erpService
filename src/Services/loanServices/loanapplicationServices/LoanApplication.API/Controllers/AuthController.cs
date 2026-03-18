using Microsoft.AspNetCore.Mvc;
using LoanApplication.API.Authentication;

namespace LoanApplication.API.Controllers;

/// <summary>
/// Authentication controller for JWT token management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IJwtTokenService jwtTokenService, ILogger<AuthController> logger)
    {
        _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get JWT token for authentication
    /// </summary>
    /// <param name="request">Login request with user ID and role</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>JWT token</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login request for user {UserId}", request.UserId);

        if (string.IsNullOrWhiteSpace(request.UserId))
            return BadRequest("User ID is required");

        if (string.IsNullOrWhiteSpace(request.Role))
            request.Role = "User"; // Default role

        try
        {
            var token = _jwtTokenService.GenerateToken(request.UserId, request.Role);

            _logger.LogInformation("Token generated successfully for user {UserId}", request.UserId);

            return Ok(new TokenResponse
            {
                Token = token,
                TokenType = "Bearer",
                ExpiresIn = int.Parse("60") // Should match JWT:ExpirationMinutes
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token generation failed for user {UserId}", request.UserId);
            return BadRequest("Token generation failed");
        }
    }

    /// <summary>
    /// Validate JWT token
    /// </summary>
    /// <param name="request">Token validation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    [HttpPost("validate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ValidationResponse>> ValidateToken(
        [FromBody] ValidateTokenRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
            return BadRequest("Token is required");

        var (isValid, message) = _jwtTokenService.ValidateToken(request.Token);

        if (!isValid)
            return Unauthorized(new ValidationResponse { IsValid = false, Message = message });

        return Ok(new ValidationResponse { IsValid = true, Message = "Token is valid" });
    }
}

/// <summary>
/// Login request model
/// </summary>
public class LoginRequest
{
    public string UserId { get; set; } = string.Empty;
    public string? Role { get; set; }
    public string? Password { get; set; } // For demonstration - in real app, authenticate against credential store
}

/// <summary>
/// Token response model
/// </summary>
public class TokenResponse
{
    public string Token { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
}

/// <summary>
/// Token validation request model
/// </summary>
public class ValidateTokenRequest
{
    public string Token { get; set; } = string.Empty;
}

/// <summary>
/// Token validation response model
/// </summary>
public class ValidationResponse
{
    public bool IsValid { get; set; }
    public string? Message { get; set; }
}
