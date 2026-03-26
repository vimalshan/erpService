using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AimsTransactionService.Application.Leaves.Commands.ApplyLeave;
using AimsTransactionService.Application.Leaves.Commands.ApproveLeave;
using AimsTransactionService.Application.Leaves.Queries.GetLeavesByEmployee;
using AimsTransactionService.Application.Leaves.Queries.GetLeaveBalance;

namespace AimsTransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class LeavesController(ISender sender) : ControllerBase
{
    [HttpGet("employee/{employeeSysId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long employeeSysId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetLeavesByEmployeeQuery(employeeSysId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("balance/{employeeSysId:long}/{leaveId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBalance(long employeeSysId, int leaveId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetLeaveBalanceQuery(employeeSysId, leaveId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Apply([FromBody] ApplyLeaveCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("{id:long}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(
        long id, [FromBody] ApproveLeaveCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command with { LeaveDetailId = id }, cancellationToken);
        return NoContent();
    }
}
