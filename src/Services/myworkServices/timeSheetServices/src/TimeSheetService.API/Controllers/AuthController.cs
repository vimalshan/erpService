using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSheetService.API.Authentication;

namespace TimeSheetService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AuthController(TokenService tokenService) => _tokenService = tokenService;

    [HttpPost("token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    public ActionResult<TokenResponse> GenerateToken([FromBody] TokenRequest request)
    {
        var token = _tokenService.GenerateToken(request.UserId, request.UserName, request.Roles);
        return Ok(new TokenResponse { Token = token });
    }
}

public record TokenRequest
{
    public string UserId { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string[]? Roles { get; init; }
}

public record TokenResponse
{
    public string Token { get; init; } = string.Empty;
}
