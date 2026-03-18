using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeAttendance.Application.AbsenteeismMis.Commands.CreateAbsenteeismMis;
using TimeAttendance.Application.AbsenteeismMis.Commands.DeleteAbsenteeismMis;
using TimeAttendance.Application.AbsenteeismMis.Commands.UpdateAbsenteeismMis;
using TimeAttendance.Application.AbsenteeismMis.Queries.GetAbsenteeismMis;
using TimeAttendance.Application.AbsenteeismMis.Queries.GetAllAbsenteeismMis;

namespace TimeAttendance.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class AbsenteeismMisController(IMediator mediator) : ControllerBase
{
    /// <summary>Gets all absenteeism MIS records with pagination.</summary>
    [HttpGet]
    [Authorize(Policy = "ReadPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? unitId = null,
        [FromQuery] string? month = null,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new GetAllAbsenteeismMisQuery(pageNumber, pageSize, unitId, month), cancellationToken);
        return Ok(result);
    }

    /// <summary>Gets a single absenteeism MIS record by ID.</summary>
    [HttpGet("{id:long}")]
    [Authorize(Policy = "ReadPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAbsenteeismMisQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Creates a new absenteeism MIS record.</summary>
    [HttpPost]
    [Authorize(Policy = "WritePolicy")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAbsenteeismMisCommand command,
        CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    /// <summary>Updates an existing absenteeism MIS record.</summary>
    [HttpPut("{id:long}")]
    [Authorize(Policy = "WritePolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] UpdateAbsenteeismMisCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest("ID mismatch.");
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>Deletes an absenteeism MIS record.</summary>
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "WritePolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteAbsenteeismMisCommand(id), cancellationToken);
        return NoContent();
    }
}
