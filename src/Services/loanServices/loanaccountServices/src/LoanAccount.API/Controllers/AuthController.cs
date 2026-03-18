using LoanAccount.API.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanAccount.API.Controllers;

/// <summary>
/// Authentication controller for JWT token management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _tokenService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IJwtTokenService tokenService, IConfiguration configuration, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Authenticate user and obtain JWT token
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] TokenRequest request)
    {
        // In production, validate against user database
        // For demo, accept any non-empty credentials
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            _logger.LogWarning("Login attempt with empty credentials");
            return Unauthorized(new ApiErrorResponse
            {
                Message = "Invalid username or password"
            });
        }

        // Demo: Accept demo/demo credentials
        if (request.Username != "demo" || request.Password != "demo")
        {
            _logger.LogWarning("Login attempt with invalid credentials for user {Username}", request.Username);
            return Unauthorized(new ApiErrorResponse
            {
                Message = "Invalid username or password"
            });
        }

        var roles = new[] { "User", "LoanManager" };
        var token = _tokenService.GenerateToken("1", request.Username, roles);

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var expirationMinutes = jwtSettings.GetValue<int>("ExpirationMinutes");

        _logger.LogInformation("User {Username} logged in successfully", request.Username);

        return Ok(new ApiResponse<TokenResponse>
        {
            Data = new TokenResponse
            {
                AccessToken = token,
                ExpiresIn = expirationMinutes * 60
            }
        });
    }

    /// <summary>
    /// Validate and decode JWT token
    /// </summary>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public IActionResult Validate([FromHeader] string authorization)
    {
        if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
        {
            return Unauthorized(new ApiErrorResponse
            {
                Message = "Missing or invalid authorization header"
            });
        }

        var token = authorization.Substring("Bearer ".Length);
        var principal = _tokenService.ValidateToken(token);

        if (principal is null)
        {
            _logger.LogWarning("Token validation failed");
            return Unauthorized(new ApiErrorResponse
            {
                Message = "Invalid or expired token"
            });
        }

        var claims = principal.Claims.Select(c => new { c.Type, c.Value });

        return Ok(new ApiResponse<object>
        {
            Data = new { IsValid = true, Claims = claims }
        });
    }
}

/// <summary>
/// Token response from authentication
/// </summary>
public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
}
