using LeaveServices.Application.Features.LeaveRequests.Commands;
using LeaveServices.Application.Features.LeaveRequests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class LeaveRequestsController : ControllerBase
{
    private readonly IMediator _mediator;
    public LeaveRequestsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get a leave request by ID.</summary>
    [HttpGet("{reqNum:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long reqNum, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLeaveRequestByIdQuery(reqNum), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get all leave requests for an employee.</summary>
    [HttpGet("employee/{empUserId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(string empUserId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLeaveRequestsByEmployeeQuery(empUserId), ct);
        return Ok(result);
    }

    /// <summary>Create a new leave request.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateLeaveRequestCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { reqNum = result.ReqNum }, result);
    }

    /// <summary>Add a detail line to a leave request.</summary>
    [HttpPost("{reqNum:long}/details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddDetail(long reqNum, [FromBody] AddLeaveRequestDetailCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command with { ReqNum = reqNum }, ct);
        return Ok(result);
    }
}
