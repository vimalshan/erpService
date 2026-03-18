using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentService.Application.DTOs;
using RecruitmentService.Application.Interfaces;
using RecruitmentService.Domain.Interfaces;

namespace RecruitmentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IProspectRepository _prospectRepository;
    private readonly ITokenService _tokenService;

    public AuthController(IProspectRepository prospectRepository, ITokenService tokenService)
    {
        _prospectRepository = prospectRepository;
        _tokenService = tokenService;
    }

    /// <summary>Login with prospect credentials and receive a JWT token.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var prospect = await _prospectRepository.GetByEmailAsync(request.Email, ct);
        if (prospect == null)
            return Unauthorized(new { message = "Invalid credentials." });

        // NOTE: In production, use hashed password comparison (BCrypt/Argon2).
        // The WEBPROSPECT_MAST schema stores a short password field.
        // This is a placeholder for integration with a proper credential store.
        var token = _tokenService.GenerateToken(
            prospect.WebUserId,
            prospect.EmailId ?? string.Empty,
            new[] { "Prospect" });

        return Ok(new AuthResponse(token, prospect.WebUserId, prospect.FullName, prospect.EmailId ?? string.Empty));
    }
}
