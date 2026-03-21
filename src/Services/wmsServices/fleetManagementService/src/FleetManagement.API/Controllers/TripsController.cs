using FleetManagement.Application.Commands.Trips;
using FleetManagement.Application.DTOs;
using FleetManagement.Application.Queries.Trips;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TripsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TripDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllTripsQuery(), ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TripDto>> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTripByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:int}/stops")]
    public async Task<ActionResult<TripDto>> GetWithStops(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTripWithStopsQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IReadOnlyList<TripDto>>> GetByStatus(string status, CancellationToken ct)
        => Ok(await mediator.Send(new GetTripsByStatusQuery(status), ct));

    [HttpPost]
    public async Task<ActionResult<TripDto>> Create([FromBody] CreateTripCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.TripId }, result);
    }

    [HttpPost("{id:int}/start")]
    public async Task<ActionResult<TripDto>> Start(int id, [FromBody] StartTripCommand command, CancellationToken ct)
    {
        if (id != command.TripId) return BadRequest();
        return Ok(await mediator.Send(command, ct));
    }

    [HttpPost("{id:int}/complete")]
    public async Task<ActionResult<TripDto>> Complete(int id, [FromBody] CompleteTripCommand command, CancellationToken ct)
    {
        if (id != command.TripId) return BadRequest();
        return Ok(await mediator.Send(command, ct));
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<ActionResult<TripDto>> Cancel(int id, CancellationToken ct)
        => Ok(await mediator.Send(new CancelTripCommand(id), ct));
}
