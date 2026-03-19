using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileAppManagement.Application.Commands;
using MobileAppManagement.Application.Queries;

namespace MobileAppManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoginsController(IMediator mediator) : ControllerBase
{
    [HttpGet("user/{userSysId}")]
    public async Task<IActionResult> GetByUser(decimal userSysId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLoginsByUserQuery(userSysId), ct);
        return Ok(result);
    }

    [HttpGet("{loginId}")]
    public async Task<IActionResult> GetById(decimal loginId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLoginByIdQuery(loginId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> LogLogin([FromBody] LogUserLoginCommand command, CancellationToken ct)
    {
        var loginId = await mediator.Send(command, ct);
        return Ok(new { loginId });
    }
}
