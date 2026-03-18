using CompensationBenefits.Application.Features.MobileConnections;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompensationBenefits.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MobileConnectionsController(IMediator mediator) : ControllerBase
{
    [HttpGet("employee/{empSysId:long}")]
    public async Task<IActionResult> GetByEmployee(long empSysId, CancellationToken ct)
        => Ok(await mediator.Send(new GetMobileConnectionsByEmployeeQuery(empSysId), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMobileConnectionCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return Created($"api/mobileconnections/{id}", new { id });
    }
}
