using BusServices.Application.EmployeeAssignments.Commands;
using BusServices.Application.EmployeeAssignments.Queries;
using BusServices.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class EmployeeBusController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeBusController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get assignments for an employee.</summary>
    [HttpGet("employee/{empSysId:long}")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeBusDto>), 200)]
    public async Task<IActionResult> GetByEmployee(long empSysId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetAssignmentsByEmployeeQuery(empSysId), ct));

    /// <summary>Assign an employee to a bus route.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(EmployeeBusDto), 201)]
    public async Task<IActionResult> Assign([FromBody] AssignEmployeeToBusCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return StatusCode(201, result);
    }

    /// <summary>Close an employee bus assignment.</summary>
    [HttpPut("{empBusId:long}/close")]
    [ProducesResponseType(typeof(EmployeeBusDto), 200)]
    public async Task<IActionResult> Close(long empBusId, [FromBody] CloseAssignmentBody body, CancellationToken ct)
    {
        var result = await _mediator.Send(new CloseEmployeeAssignmentCommand(empBusId, body.ClosingDate, body.ModifiedBy), ct);
        return Ok(result);
    }
}

public record CloseAssignmentBody(DateTime ClosingDate, long ModifiedBy);
