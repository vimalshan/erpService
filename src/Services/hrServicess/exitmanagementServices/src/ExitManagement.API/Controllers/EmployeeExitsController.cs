using ExitManagement.Application.Features.EmployeeExits.Commands;
using ExitManagement.Application.Features.EmployeeExits.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExitManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class EmployeeExitsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeExitsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Gets all employee exits.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllExitsQuery(), ct);
        return Ok(result);
    }

    /// <summary>Gets an employee exit by exit number.</summary>
    [HttpGet("{exitNo:decimal}")]
    public async Task<IActionResult> GetById(decimal exitNo, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetExitByIdQuery(exitNo), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Gets all exits for an employee.</summary>
    [HttpGet("employee/{employeeSysId:decimal}")]
    public async Task<IActionResult> GetByEmployee(decimal employeeSysId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetExitsByEmployeeQuery(employeeSysId), ct);
        return Ok(result);
    }

    /// <summary>Initiates a new employee exit.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExitCommand command, CancellationToken ct)
    {
        var exitNo = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { exitNo }, new { ExitNo = exitNo });
    }

    /// <summary>Approves an employee exit.</summary>
    [HttpPost("{exitNo:decimal}/approve")]
    [Authorize(Roles = "HR,Manager")]
    public async Task<IActionResult> Approve(decimal exitNo, [FromBody] decimal approvedBy, CancellationToken ct)
    {
        var success = await _mediator.Send(new ApproveExitCommand(exitNo, approvedBy), ct);
        return success ? Ok(new { message = "Exit approved." }) : NotFound();
    }

    /// <summary>Revokes an employee exit.</summary>
    [HttpPost("{exitNo:decimal}/revoke")]
    [Authorize(Roles = "HR,Manager")]
    public async Task<IActionResult> Revoke(decimal exitNo, [FromBody] RevokeExitRequest req, CancellationToken ct)
    {
        var success = await _mediator.Send(new RevokeExitCommand(exitNo, req.Reason, req.RevokedBy), ct);
        return success ? Ok(new { message = "Exit revoked." }) : NotFound();
    }
}

public record RevokeExitRequest(string Reason, decimal RevokedBy);
