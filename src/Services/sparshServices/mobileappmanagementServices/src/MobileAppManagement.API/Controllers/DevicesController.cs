using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileAppManagement.Application.Commands;
using MobileAppManagement.Application.Queries;

namespace MobileAppManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DevicesController(IMediator mediator) : ControllerBase
{
    [HttpGet("employee/{employeeSysId}")]
    public async Task<IActionResult> GetByEmployee(decimal employeeSysId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDevicesByEmployeeQuery(employeeSysId), ct);
        return Ok(result);
    }

    [HttpGet("{employeeSysId}/{deviceId}")]
    public async Task<IActionResult> GetByKey(decimal employeeSysId, string deviceId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDeviceByKeyQuery(employeeSysId, deviceId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDeviceCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(new { message = result });
    }

    [HttpPost("deactivate")]
    public async Task<IActionResult> Deactivate([FromBody] DeactivateDeviceCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(new { message = result });
    }
}
