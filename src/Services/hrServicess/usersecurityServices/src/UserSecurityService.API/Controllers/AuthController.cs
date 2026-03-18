using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserSecurityService.Application.DTOs;
using UserSecurityService.Application.Features.Auth.Commands;

namespace UserSecurityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>Authenticates a user and returns a JWT bearer token.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthTokenDto>> Login([FromBody] LoginRequestDto dto, CancellationToken ct)
    {
        var token = await mediator.Send(new LoginCommand(dto.UserId, dto.Password), ct);
        return Ok(token);
    }
}
