using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RackingSystem.Application.Features.Shelves.Commands;
using RackingSystem.Application.Features.Shelves.DTOs;
using RackingSystem.Application.Features.Shelves.Queries;

namespace RackingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class ShelvesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ShelvesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("rack/{rackId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ShelfDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRack(int rackId, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetShelvesByRackQuery(rackId), ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ShelfDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetShelfByIdQuery(id), ct));

    [HttpPost]
    [ProducesResponseType(typeof(ShelfDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateShelfCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ShelfDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateShelfCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest("Route id and body id must match.");
        return Ok(await _mediator.Send(command, ct));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteShelfCommand(id), ct);
        return NoContent();
    }
}
