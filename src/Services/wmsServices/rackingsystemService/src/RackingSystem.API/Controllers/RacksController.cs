using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RackingSystem.Application.Features.Racks.Commands.CreateRack;
using RackingSystem.Application.Features.Racks.Commands.DeleteRack;
using RackingSystem.Application.Features.Racks.Commands.UpdateRack;
using RackingSystem.Application.Features.Racks.DTOs;
using RackingSystem.Application.Features.Racks.Queries.GetRackById;
using RackingSystem.Application.Features.Racks.Queries.GetRacks;

namespace RackingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class RacksController : ControllerBase
{
    private readonly IMediator _mediator;
    public RacksController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all racks, optionally filtered by zone.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RackDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int? zoneId, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetRacksQuery(zoneId), ct));

    /// <summary>Get a rack by ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RackDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetRackByIdQuery(id), ct));

    /// <summary>Create a new rack.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(RackDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRackCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update an existing rack.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(RackDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRackCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest("Route id and body id must match.");
        return Ok(await _mediator.Send(command, ct));
    }

    /// <summary>Deactivate a rack (soft delete).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteRackCommand(id), ct);
        return NoContent();
    }
}
