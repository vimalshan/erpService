using FleetManagement.Application.Commands.Routes;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Queries.Routes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoutesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RouteDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllRoutesQuery(), ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RouteDto>> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetRouteByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<RouteDto>> Create([FromBody] CreateRouteCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.RouteId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RouteDto>> Update(int id, [FromBody] UpdateRouteCommand command, CancellationToken ct)
    {
        if (id != command.RouteId) return BadRequest();
        return Ok(await mediator.Send(command, ct));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteRouteCommand(id), ct);
        return result ? NoContent() : NotFound();
    }
}
