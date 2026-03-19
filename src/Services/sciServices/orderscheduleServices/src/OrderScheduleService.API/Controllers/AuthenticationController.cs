namespace OrderScheduleService.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderScheduleService.API.Services;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(ITokenService tokenService, ILogger<AuthenticationController> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// Generate JWT token for authentication
    /// </summary>
    [HttpPost("token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GenerateToken([FromBody] TokenRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.UserName))
            {
                return BadRequest(new { message = "UserId and UserName are required" });
            }

            var roles = request.Roles ?? new[] { "User" };
            var token = _tokenService.GenerateToken(request.UserId, request.UserName, roles);

            _logger.LogInformation($"Token generated for user {request.UserName}");

            return Ok(new TokenResponse
            {
                Token = token,
                ExpiresIn = 3600,
                TokenType = "Bearer"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating token");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Validate JWT token
    /// </summary>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(ValidateTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult ValidateToken([FromBody] ValidateTokenRequest request)
    {
        try
        {
            var principal = _tokenService.ValidateToken(request.Token);
            if (principal == null)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            _logger.LogInformation("Token validated successfully");

            return Ok(new ValidateTokenResponse
            {
                IsValid = true,
                UserId = principal.FindFirst("sub")?.Value,
                UserName = principal.FindFirst("name")?.Value
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}

public class TokenRequest
{
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string[]? Roles { get; set; }
}

public class TokenResponse
{
    public string Token { get; set; } = null!;
    public int ExpiresIn { get; set; }
    public string TokenType { get; set; } = null!;
}

public class ValidateTokenRequest
{
    public string Token { get; set; } = null!;
}

public class ValidateTokenResponse
{
    public bool IsValid { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
}
