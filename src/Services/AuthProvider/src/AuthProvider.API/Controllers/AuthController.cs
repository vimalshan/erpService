using AuthProvider.Application.Commands;
using AuthProvider.Application.DTOs;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthProvider.API.Controllers;

/// <summary>
/// Authentication controller – v1 and v2 endpoints.
/// Demonstrates API Versioning + Authentication + Authorization + ILogger.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>Register a new user account.</summary>
    [HttpPost("register")]
    [AllowAnonymous]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] CreateUserDto dto,
        CancellationToken ct)
    {
        _logger.LogInformation("Register request for {Email}", dto.Email);

        var result = await _mediator.Send(
            new CreateUserCommand(dto.Username, dto.Email, dto.Password, dto.FirstName, dto.LastName), ct);

        return CreatedAtAction(nameof(GetMe), new { }, result);
    }

    /// <summary>Authenticate and receive JWT tokens.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto dto,
        CancellationToken ct)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        _logger.LogInformation("Login attempt for {UsernameOrEmail} from {IpAddress}", dto.UsernameOrEmail, ipAddress);

        var result = await _mediator.Send(new LoginCommand(dto.UsernameOrEmail, dto.Password, ipAddress), ct);
        return Ok(result);
    }

    /// <summary>Refresh an expired access token using a refresh token.</summary>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequest dto,
        CancellationToken ct)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var result = await _mediator.Send(new RefreshTokenCommand(dto.RefreshToken, ip), ct);
        return Ok(result);
    }

    /// <summary>Revoke a refresh token.</summary>
    [HttpPost("revoke-token")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RevokeToken(
        [FromBody] RevokeTokenRequest dto,
        CancellationToken ct)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        await _mediator.Send(new RevokeTokenCommand(dto.RefreshToken, ip), ct);
        return NoContent();
    }

    /// <summary>Get the currently authenticated user's profile.</summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMe(CancellationToken ct)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userId, out var guid)) return Unauthorized();

        var result = await _mediator.Send(new Application.Queries.GetUserByIdQuery(guid), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>v2 login – returns additional user metadata.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginV2(
        [FromBody] LoginRequestDto dto,
        CancellationToken ct)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var result = await _mediator.Send(new LoginCommand(dto.UsernameOrEmail, dto.Password, ip), ct);

        // v2 enriches the response with API version info
        return Ok(new { result.AccessToken, result.RefreshToken, result.ExpiresAt, ApiVersion = "2.0" });
    }
}

public record RefreshTokenRequest(string RefreshToken);
public record RevokeTokenRequest(string RefreshToken);
