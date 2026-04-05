using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSheetService.Application.Commands.CreateTcProject;
using TimeSheetService.Application.Commands.SubmitTcTimesheet;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Application.Queries.GetTcProjectById;
using TimeSheetService.Application.Queries.GetTcProjects;

namespace TimeSheetService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TcProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TcProjectsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TcProjectDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TcProjectDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTcProjectsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TcProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TcProjectDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTcProjectByIdQuery(id), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TcProjectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TcProjectDto>> Create([FromBody] CreateTcProjectCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.ProjectId }, result);
    }
}

[ApiController]
[Route("api/tc-timesheets")]
[Authorize]
public class TcTimesheetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TcTimesheetsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(TcTimesheetEntryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TcTimesheetEntryDto>> Submit([FromBody] SubmitTcTimesheetCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Created($"/api/tc-timesheets/{result.TimeId}", result);
    }
}
