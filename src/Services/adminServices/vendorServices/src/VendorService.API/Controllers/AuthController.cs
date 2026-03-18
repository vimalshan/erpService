using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorService.Application.Commands;
using VendorService.Application.Queries;

namespace VendorService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
    // Placeholder: In production, integrate with Identity or external IdP
    [AllowAnonymous]
    [HttpPost("token")]
    public IActionResult Token([FromBody] LoginRequest request)
    {
        // For demo only — replace with real auth logic
        if (request.Username == "admin" && request.Password == "admin")
        {
            return Ok(new { token = "replace-with-real-jwt" });
        }
        return Unauthorized();
    }
}

public sealed record LoginRequest(string Username, string Password);
