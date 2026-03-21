using FleetManagement.Application.Commands.Drivers;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Queries.Drivers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DriversController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DriverDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllDriversQuery(), ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DriverDto>> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDriverByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IReadOnlyList<DriverDto>>> GetActive(CancellationToken ct)
        => Ok(await mediator.Send(new GetActiveDriversQuery(), ct));

    [HttpPost]
    public async Task<ActionResult<DriverDto>> Create([FromBody] CreateDriverCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.DriverId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<DriverDto>> Update(int id, [FromBody] UpdateDriverCommand command, CancellationToken ct)
    {
        if (id != command.DriverId) return BadRequest();
        return Ok(await mediator.Send(command, ct));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteDriverCommand(id), ct);
        return result ? NoContent() : NotFound();
    }
}
