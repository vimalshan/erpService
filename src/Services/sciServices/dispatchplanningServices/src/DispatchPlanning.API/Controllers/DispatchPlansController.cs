using DispatchPlanning.Application.DTOs;
using DispatchPlanning.Application.Features.DispatchPlans.Commands;
using DispatchPlanning.Application.Features.DispatchPlans.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DispatchPlanning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DispatchPlansController : ControllerBase
{
    private readonly IMediator _mediator;

    public DispatchPlansController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{companyUnitId:int}")]
    [ProducesResponseType(typeof(IEnumerable<DispatchPlanHeaderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(int companyUnitId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllDispatchPlansQuery(companyUnitId), ct);
        return Ok(result);
    }

    [HttpGet("{id:int}/detail")]
    [ProducesResponseType(typeof(DispatchPlanDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDispatchPlanByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateDispatchPlanCommand command, CancellationToken ct)
    {
        var id = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPost("{id:int}/items")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItem(int id, [FromBody] AddDispatchPlanItemCommand command, CancellationToken ct)
    {
        if (id != command.PlanHeaderId) return BadRequest("Route ID does not match body.");
        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPut("{id:int}/forecasts")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateForecasts(int id,
        [FromBody] UpdateDispatchPlanForecastCommand command, CancellationToken ct)
    {
        if (id != command.PlanHeaderId) return BadRequest("Route ID does not match body.");
        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id, [FromQuery] int deletedBy, CancellationToken ct)
    {
        await _mediator.Send(new DeleteDispatchPlanCommand(id, deletedBy), ct);
        return NoContent();
    }

    [HttpPost("{id:int}/subgroup-targets")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddSubGroupTarget(int id,
        [FromBody] AddSubGroupTargetCommand command, CancellationToken ct)
    {
        if (id != command.PlanHeaderId) return BadRequest("Route ID does not match body.");
        await _mediator.Send(command, ct);
        return NoContent();
    }
}
