using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationSetup.Application.DTOs;
using OrganizationSetup.Application.UserMaps.Commands;
using OrganizationSetup.Application.UserMaps.Queries;

namespace OrganizationSetup.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserMapsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserMapsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("org/{orgId}")]
    public async Task<ActionResult<IEnumerable<UserMapDto>>> GetByOrg(long orgId, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetUserMapsByOrgQuery(orgId), ct));

    [HttpGet("employee/{empSysId}")]
    public async Task<ActionResult<IEnumerable<UserMapDto>>> GetByEmployee(long empSysId, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetUserMapsByEmployeeQuery(empSysId), ct));

    [HttpPost]
    public async Task<ActionResult<UserMapDto>> CreateUserMap([FromBody] CreateUserMapCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}
