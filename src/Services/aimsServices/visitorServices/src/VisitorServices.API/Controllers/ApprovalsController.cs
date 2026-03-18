using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitorServices.Application.Approvals.Commands.ProcessApproval;
using VisitorServices.Application.Approvals.Queries.GetPendingApprovals;

namespace VisitorServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ApprovalsController(ISender sender) : ControllerBase
{
    /// <summary>Get all pending approvals for a given approver.</summary>
    [HttpGet("pending")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending([FromQuery] long approverId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPendingApprovalsQuery(approverId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Approve or reject a visitor approval request.</summary>
    [HttpPost("{id:long}/process")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Process(long id, [FromBody] ProcessApprovalCommand command, CancellationToken cancellationToken)
    {
        var actualCommand = command with { RequestId = id };
        var result = await sender.Send(actualCommand, cancellationToken);
        return Ok(result);
    }
}
