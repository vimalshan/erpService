using BusServices.Application.Buses.Commands;
using BusServices.Application.Buses.Queries;
using BusServices.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class BusesController : ControllerBase
{
    private readonly IMediator _mediator;

    public BusesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all buses.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BusDto>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetBusesQuery(), ct));

    /// <summary>Get a bus by ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BusDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBusByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Register a new bus.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BusDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register([FromBody] RegisterBusCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.BusId }, result);
    }

    /// <summary>Update bus details.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BusDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBusCommandBody body, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateBusCommand(id, body.Description, body.Capacity, body.ModifiedBy), ct);
        return Ok(result);
    }
}

public record UpdateBusCommandBody(string? Description, int Capacity, long ModifiedBy);
