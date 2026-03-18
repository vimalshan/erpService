using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeService.Application.Commands.AssignApprover;
using EmployeeService.Application.Queries.GetApprovers;

namespace EmployeeService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class ApproverController : ControllerBase
{
    private readonly IMediator _mediator;
    public ApproverController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all approvers for an employee.</summary>
    [HttpGet("employee/{empSysId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long empSysId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetApproversByEmployeeQuery(empSysId), ct);
        return Ok(result);
    }

    /// <summary>Assign a new approver to an employee.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Assign([FromBody] AssignApproverCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Created($"api/approver/{result.ApproverId}", result);
    }
}
