using InvoiceProcessing.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceProcessing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IJwtTokenService tokenService) : ControllerBase
{
    public record LoginRequest(string Username, string Password);
    public record LoginResponse(string Token, DateTime Expiry);

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        // In production, validate credentials against a user store
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            return Unauthorized("Invalid credentials");

        var token = tokenService.GenerateToken(1, request.Username, ["User"]);
        return Ok(new LoginResponse(token, DateTime.UtcNow.AddMinutes(60)));
    }
}
