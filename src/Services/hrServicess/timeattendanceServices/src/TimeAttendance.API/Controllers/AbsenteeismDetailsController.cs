using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeAttendance.Application.AbsenteeismDetails.Commands.CreateAbsenteeismDetail;
using TimeAttendance.Application.AbsenteeismDetails.Commands.DeleteAbsenteeismDetail;
using TimeAttendance.Application.AbsenteeismDetails.Commands.UpdateAbsenteeismDetail;
using TimeAttendance.Application.AbsenteeismDetails.Queries.GetAbsenteeismDetail;
using TimeAttendance.Application.AbsenteeismDetails.Queries.GetAbsenteeismDetailByPeriod;
using TimeAttendance.Application.AbsenteeismDetails.Queries.GetAllAbsenteeismDetails;

namespace TimeAttendance.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class AbsenteeismDetailsController(IMediator mediator) : ControllerBase
{
    /// <summary>Gets all absenteeism detail records with pagination.</summary>
    [HttpGet]
    [Authorize(Policy = "ReadPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] long? unitId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new GetAllAbsenteeismDetailsQuery(pageNumber, pageSize, unitId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Gets a single absenteeism detail record by ID.</summary>
    [HttpGet("{id:long}")]
    [Authorize(Policy = "ReadPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAbsenteeismDetailQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Gets absenteeism details by unit and period.</summary>
    [HttpGet("unit/{unitId:long}/period/{year:int}/{month:int}")]
    [Authorize(Policy = "ReadPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPeriod(
        long unitId, int year, int month, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetAbsenteeismDetailByPeriodQuery(unitId, year, month), cancellationToken);
        return Ok(result);
    }

    /// <summary>Creates a new absenteeism detail record.</summary>
    [HttpPost]
    [Authorize(Policy = "WritePolicy")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAbsenteeismDetailCommand command,
        CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    /// <summary>Updates an existing absenteeism detail record.</summary>
    [HttpPut("{id:long}")]
    [Authorize(Policy = "WritePolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] UpdateAbsenteeismDetailCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest("ID mismatch.");
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>Deletes an absenteeism detail record.</summary>
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "WritePolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteAbsenteeismDetailCommand(id), cancellationToken);
        return NoContent();
    }
}
