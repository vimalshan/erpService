using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeaveServices.Application.Commands.Leave;
using LeaveServices.Application.Queries.Leave;

namespace LeaveServices.API.Controllers;

[ApiController]
[Route("api/leave-approvals")]
[Authorize(Roles = "Admin,HR,Approver")]
[Produces("application/json")]
public sealed class LeaveApprovalController : ControllerBase
{
    private readonly IMediator _mediator;
    public LeaveApprovalController(IMediator mediator) => _mediator = mediator;

    /// <summary>Approve, reject, or cancel a leave application.</summary>
    [HttpPost]
    public async Task<IActionResult> Process([FromBody] ApproveLeaveCommand cmd, CancellationToken ct)
    {
        await _mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>Get approval history for a leave application.</summary>
    [HttpGet("{leaveDetailId:long}/history")]
    public async Task<IActionResult> GetHistory(long leaveDetailId, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetLeaveApprovalHistoryQuery(leaveDetailId), ct));
}

[ApiController]
[Route("api/leave-credits")]
[Authorize(Roles = "Admin,HR")]
[Produces("application/json")]
public sealed class LeaveCreditController : ControllerBase
{
    private readonly IMediator _mediator;
    public LeaveCreditController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Credit([FromBody] CreditLeaveCommand cmd, CancellationToken ct)
    {
        var id = await _mediator.Send(cmd, ct);
        return Ok(new { id });
    }
}

[ApiController]
[Route("api/compoff")]
[Authorize]
[Produces("application/json")]
public sealed class CompOffController : ControllerBase
{
    private readonly IMediator _mediator;
    public CompOffController(IMediator mediator) => _mediator = mediator;

    [HttpGet("employee/{empId:long}")]
    public async Task<IActionResult> GetAvailable(long empId, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetCompOffByEmployeeQuery(empId), ct));

    [HttpPost]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> Add([FromBody] AddCompOffCommand cmd, CancellationToken ct)
    {
        var id = await _mediator.Send(cmd, ct);
        return Ok(new { id });
    }
}
