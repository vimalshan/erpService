namespace FeedbackService.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Configuration;

/// <summary>
/// Authentication controller for obtaining JWT tokens
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _tokenService;

    /// <summary>
    /// Initializes a new instance of the AuthController class
    /// </summary>
    public AuthController(JwtTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <summary>
    /// Generates a JWT token for the specified user
    /// </summary>
    /// <param name="request">Login request</param>
    /// <returns>JWT token</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            return BadRequest(new { message = "Username and password are required" });

        // TODO: Implement actual user validation against your identity provider
        // For now, this is a placeholder that accepts any credentials
        if (!ValidateCredentials(request.Username, request.Password))
            return Unauthorized(new { message = "Invalid credentials" });

        var roles = new[] { "User" }; // TODO: Get actual roles from your identity provider
        var token = _tokenService.GenerateToken(request.Username, request.Username, roles);

        return Ok(new LoginResponse { Token = token, ExpiresIn = 3600 });
    }

    /// <summary>
    /// Validates user credentials (placeholder implementation)
    /// </summary>
    private static bool ValidateCredentials(string username, string password)
    {
        // TODO: Replace with actual validation logic (database, LDAP, etc.)
        // For development purposes, accept demo credentials
        return username == "admin" && password == "password" ||
               username == "user" && password == "password";
    }
}

/// <summary>
/// Login request model
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Gets or sets the username
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the password
    /// </summary>
    public string? Password { get; set; }
}

/// <summary>
/// Login response model
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// Gets or sets the JWT token
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Gets or sets the token expiration time in seconds
    /// </summary>
    public int ExpiresIn { get; set; }
}
