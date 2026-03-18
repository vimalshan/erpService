using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using EligibilityService.Application.DTOs;
using EligibilityService.Application.Commands.EligibilityMaster;
using EligibilityService.Domain.Interfaces;

namespace EligibilityService.API.Controllers;

/// <summary>Auth controller — issues JWT tokens for testing.</summary>
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config) => _config = config;

    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Token([FromBody] LoginRequest request)
    {
        // Intentionally minimal — replace with real user store in production
        var adminUser = _config["Auth:AdminUsername"] ?? "admin";
        var adminPass = _config["Auth:AdminPassword"] ?? "Admin@123";

        if (request.Username != adminUser || request.Password != adminPass)
            return Unauthorized(new { message = "Invalid credentials." });

        var token = JwtTokenHelper.Generate(_config, request.Username);
        return Ok(new TokenResponse(token, 3600));
    }
}

public record LoginRequest(string Username, string Password);
public record TokenResponse(string Token, int ExpiresIn);
