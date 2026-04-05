using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSheetService.Application.Commands.DeleteTimesheet;
using TimeSheetService.Application.Commands.SubmitTimesheet;
using TimeSheetService.Application.Commands.UpdateTimesheet;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Application.Queries.GetAllTimesheets;
using TimeSheetService.Application.Queries.GetTimesheetById;
using TimeSheetService.Application.Queries.GetTimesheetsByEmployee;

namespace TimeSheetService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TimesheetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimesheetsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TimesheetEntryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TimesheetEntryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTimesheetsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TimesheetEntryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TimesheetEntryDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTimesheetByIdQuery(id), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("employee/{employeeSysId:long}")]
    [ProducesResponseType(typeof(IEnumerable<TimesheetEntryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TimesheetEntryDto>>> GetByEmployee(
        long employeeSysId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTimesheetsByEmployeeQuery(employeeSysId, from, to), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TimesheetEntryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TimesheetEntryDto>> Submit([FromBody] SubmitTimesheetCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.TimeId }, result);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(TimesheetEntryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TimesheetEntryDto>> Update(long id, [FromBody] UpdateTimesheetCommand command, CancellationToken cancellationToken)
    {
        if (id != command.TimeId) return BadRequest("ID mismatch");
        var result = await _mediator.Send(command, cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, [FromQuery] long modifiedBy, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteTimesheetCommand(id, modifiedBy), cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
