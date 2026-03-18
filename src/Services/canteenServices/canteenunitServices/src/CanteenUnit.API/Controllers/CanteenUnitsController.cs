using CanteenUnit.Application.Features.CanteenUnits.Commands.CreateCanteenUnit;
using CanteenUnit.Application.Features.CanteenUnits.Commands.DeleteCanteenUnit;
using CanteenUnit.Application.Features.CanteenUnits.Commands.UpdateCanteenUnit;
using CanteenUnit.Application.Features.CanteenUnits.Queries.GetAllCanteenUnits;
using CanteenUnit.Application.Features.CanteenUnits.Queries.GetCanteenUnit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CanteenUnit.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CanteenUnitsController : ControllerBase
{
    private readonly IMediator _mediator;
    public CanteenUnitsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllCanteenUnitsQuery(), ct));

    [HttpGet("{comCode:decimal}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(decimal comCode, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCanteenUnitQuery(comCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCanteenUnitCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { comCode = result.UnComCod }, result);
    }

    [HttpPut("{comCode:decimal}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(decimal comCode, [FromBody] UpdateCanteenUnitCommand command, CancellationToken ct)
    {
        if (comCode != command.ComCode) return BadRequest("Route code and body code must match.");
        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{comCode:decimal}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(decimal comCode, CancellationToken ct)
    {
        await _mediator.Send(new DeleteCanteenUnitCommand(comCode), ct);
        return NoContent();
    }
}
