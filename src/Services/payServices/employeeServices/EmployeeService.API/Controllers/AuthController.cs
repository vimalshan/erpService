using EmployeeService.API.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.API.Controllers;

/// <summary>
/// Authentication controller for JWT token generation
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenGenerator _tokenGenerator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(JwtTokenGenerator tokenGenerator, ILogger<AuthController> logger)
    {
        _tokenGenerator = tokenGenerator;
        _logger = logger;
    }

    /// <summary>
    /// Get test JWT tokens for different roles
    /// </summary>
    /// <remarks>
    /// This endpoint is for testing purposes.
    /// Returns JWT tokens for Admin, Manager, Employee, and multi-role users.
    /// </remarks>
    /// <response code="200">Test tokens generated successfully</response>
    [HttpGet("test-tokens")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Dictionary<string, TokenResponse>> GetTestTokens()
    {
        _logger.LogInformation("Test tokens requested");
        
        var testTokens = _tokenGenerator.GenerateTestTokens();
        var response = new Dictionary<string, TokenResponse>();

        foreach (var kvp in testTokens)
        {
            response[kvp.Key] = new TokenResponse
            {
                Role = kvp.Key,
                Token = kvp.Value,
                TokenType = "Bearer"
            };
        }

        return Ok(response);
    }

    /// <summary>
    /// Generate JWT token for a specific user
    /// </summary>
    /// <remarks>
    /// Generates a JWT token for the specified user with the given role.
    /// Token expires in 60 minutes by default.
    /// </remarks>
    /// <param name="request">Token request containing user details</param>
    /// <response code="200">Token generated successfully</response>
    /// <response code="400">Invalid request parameters</response>
    [HttpPost("generate-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<TokenResponse> GenerateToken([FromBody] TokenRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Token generation requested for user {UserId}", request.UserId);

        var token = _tokenGenerator.GenerateToken(
            request.UserId,
            request.UserName,
            request.Role,
            request.Email);

        return Ok(new TokenResponse
        {
            Role = request.Role,
            Token = token,
            TokenType = "Bearer"
        });
    }
}

/// <summary>
/// Request model for JWT token generation
/// </summary>
public class TokenRequest
{
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Email { get; set; } = null!;
}

/// <summary>
/// Response model for JWT token
/// </summary>
public class TokenResponse
{
    public string Role { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string TokenType { get; set; } = "Bearer";
}
