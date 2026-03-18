using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProxyModule.API.Auth;

namespace ProxyModule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    public IActionResult GenerateToken([FromBody] TokenRequest request)
    {
        // In production, validate credentials against a user store
        var token = _tokenService.GenerateToken(request.UserId, request.UserName, request.Roles);
        return Ok(new TokenResponse { Token = token });
    }
}

public class TokenRequest
{
    public long UserId { get; set; }
    public string UserName { get; set; } = default!;
    public List<string>? Roles { get; set; }
}

public class TokenResponse
{
    public string Token { get; set; } = default!;
}
