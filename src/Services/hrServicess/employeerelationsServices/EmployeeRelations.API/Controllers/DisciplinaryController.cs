using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeRelations.Application.Commands.Disciplinary;
using EmployeeRelations.Application.Queries.Disciplinary;
using EmployeeRelations.Application.DTOs;

namespace EmployeeRelations.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class DisciplinaryController : ControllerBase
{
    private readonly IMediator _mediator;
    public DisciplinaryController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all disciplinary cases.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DisciplinaryMainDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetAllDisciplinaryCasesQuery(), ct));

    /// <summary>Get a disciplinary case by Id.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(DisciplinaryMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetDisciplinaryCaseQuery(id), ct));

    /// <summary>Create a new disciplinary case.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(DisciplinaryMainDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateDisciplinaryCaseCommand cmd, CancellationToken ct)
    {
        var result = await _mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Add a disciplinary action to a case.</summary>
    [HttpPost("{mainId:long}/actions")]
    [ProducesResponseType(typeof(DisciplinaryActionDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddAction(long mainId, [FromBody] AddDisciplinaryActionCommand cmd, CancellationToken ct)
    {
        var result = await _mediator.Send(cmd with { MainId = mainId }, ct);
        return CreatedAtAction(nameof(GetById), new { id = mainId }, result);
    }

    /// <summary>Approve a disciplinary action.</summary>
    [HttpPut("{mainId:long}/actions/{actionId:long}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ApproveAction(long mainId, long actionId, [FromQuery] long approvedBy, CancellationToken ct)
    {
        await _mediator.Send(new ApproveDisciplinaryActionCommand(mainId, actionId, approvedBy), ct);
        return NoContent();
    }
}
