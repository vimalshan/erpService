using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StipendService.Application.DTOs;
using StipendService.Application.Features.StipendMaster.Commands;
using StipendService.Application.Features.StipendMaster.Queries;

namespace StipendService.API.Controllers;

[ApiController]
[Route("api/v1/stipend-master")]
[Authorize]
public class StipendMasterController : ControllerBase
{
    private readonly IMediator _mediator;

    public StipendMasterController(IMediator mediator) => _mediator = mediator;

    /// <summary>Gets all stipend master records.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StipendMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllStipendMastersQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Gets a specific stipend master by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(StipendMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetStipendMasterByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Gets the active stipend for a research category and rank.</summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(StipendMasterDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive([FromQuery] long categoryId, [FromQuery] long rankId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActiveStipendByCategoryQuery(categoryId, rankId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Creates a new stipend master record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(StipendMasterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateStipendMasterCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.StipendId }, result);
    }

    /// <summary>Updates an existing stipend master record.</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(StipendMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateStipendMasterCommand command, CancellationToken cancellationToken)
    {
        if (id != command.StipendId) return BadRequest("ID mismatch.");
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>Deactivates a stipend master record.</summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(long id, [FromQuery] long updatedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeactivateStipendMasterCommand(id, updatedBy), cancellationToken);
        return NoContent();
    }
}
