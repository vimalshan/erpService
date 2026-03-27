using Microsoft.AspNetCore.Mvc;
using TransactionProcessing.API.Auth;

namespace TransactionProcessing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(TokenService tokenService) : ControllerBase
{
    public sealed record LoginRequest(string UserId, string UserName, string Role);

    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] LoginRequest request)
    {
        var token = tokenService.GenerateToken(request.UserId, request.UserName, request.Role);
        return Ok(new { token, expiresIn = 3600 });
    }

    [HttpGet("validate")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult ValidateToken() => Ok(new
    {
        valid = true,
        user = User.Identity?.Name,
        claims = User.Claims.Select(c => new { c.Type, c.Value })
    });
}
