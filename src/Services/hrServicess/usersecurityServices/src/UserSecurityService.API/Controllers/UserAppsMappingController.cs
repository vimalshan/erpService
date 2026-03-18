using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserSecurityService.Application.DTOs;
using UserSecurityService.Application.Features.UserAppsMap;
using UserSecurityService.Application.Features.UserAppsMap.Commands;

namespace UserSecurityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserAppsMappingController(IMediator mediator) : ControllerBase
{
    [HttpGet("{empSysId:decimal}")]
    [ProducesResponseType(typeof(UserAppsMappingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserAppsMappingDto>> GetByEmpSysId(decimal empSysId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetUserAppsMappingQuery(empSysId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserAppsMappingDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<UserAppsMappingDto>> Create(
        [FromBody] CreateUserAppsMappingCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByEmpSysId), new { empSysId = result.EmpSysId }, result);
    }
}
