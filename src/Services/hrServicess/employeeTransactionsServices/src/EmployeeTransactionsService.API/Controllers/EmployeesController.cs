using EmployeeTransactionsService.Application.Features.Employees.Commands;
using EmployeeTransactionsService.Application.Features.Employees.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTransactionsService.API.Controllers;

[ApiController]
[Route("api/employees")]
[Authorize(Policy = "Reader")]
public sealed class EmployeesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int take = 50, CancellationToken cancellationToken = default)
        => Ok(await mediator.Send(new ListEmployeesQuery(take), cancellationToken));

    [HttpGet("{employeeId:decimal}")]
    public async Task<IActionResult> GetById(decimal employeeId, CancellationToken cancellationToken)
    {
        var employee = await mediator.Send(new GetEmployeeByIdQuery(employeeId), cancellationToken);
        return employee is null ? NotFound() : Ok(employee);
    }

    [HttpGet("{employeeId:decimal}/timeline")]
    public async Task<IActionResult> GetTimeline(decimal employeeId, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetEmployeeTimelineQuery(employeeId), cancellationToken));

    [HttpPost]
    [Authorize(Policy = "Writer")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employeeId = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { employeeId }, new { employeeId });
    }

    [HttpPost("{employeeId:decimal}/grade-changes")]
    [Authorize(Policy = "Writer")]
    public async Task<IActionResult> RegisterGradeChange(decimal employeeId, [FromBody] RegisterGradeChangeCommand command, CancellationToken cancellationToken)
    {
        if (employeeId != command.EmployeeId)
            return BadRequest("Employee id mismatch.");

        var changeId = await mediator.Send(command, cancellationToken);
        return Ok(new { changeId });
    }

    [HttpPost("probation/{probationId:decimal}/review")]
    [Authorize(Policy = "Writer")]
    public async Task<IActionResult> ReviewProbation(decimal probationId, [FromBody] ReviewProbationCommand command, CancellationToken cancellationToken)
    {
        if (probationId != command.ProbationId)
            return BadRequest("Probation id mismatch.");

        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}