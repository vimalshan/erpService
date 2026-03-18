using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReimbursementService.Application.DTOs;
using ReimbursementService.Application.Features.Reimbursements.Commands.ApproveReimbursement;
using ReimbursementService.Application.Features.Reimbursements.Commands.CreateReimbursement;
using ReimbursementService.Application.Features.Reimbursements.Commands.MarkAsPaid;
using ReimbursementService.Application.Features.Reimbursements.Commands.RejectReimbursement;
using ReimbursementService.Application.Features.Reimbursements.Commands.SubmitReimbursement;
using ReimbursementService.Application.Features.Reimbursements.Commands.UpdateReimbursement;
using ReimbursementService.Application.Features.Reimbursements.Queries.GetAllReimbursements;
using ReimbursementService.Application.Features.Reimbursements.Queries.GetReimbursementById;
using ReimbursementService.Application.Features.Reimbursements.Queries.GetReimbursementsByEmployee;
using ReimbursementService.Application.Features.Reimbursements.Queries.GetReimbursementSummary;

namespace ReimbursementService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class ReimbursementsController(ISender mediator) : ControllerBase
{
    // GET api/reimbursements?pageNumber=1&pageSize=20
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReimbursementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => Ok(await mediator.Send(new GetAllReimbursementsQuery(pageNumber, pageSize), ct));

    // GET api/reimbursements/{id}
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ReimbursementDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetReimbursementByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // GET api/reimbursements/employee/{empSysId}
    [HttpGet("employee/{empSysId:long}")]
    [ProducesResponseType(typeof(IEnumerable<ReimbursementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long empSysId, CancellationToken ct = default)
        => Ok(await mediator.Send(new GetReimbursementsByEmployeeQuery(empSysId), ct));

    // GET api/reimbursements/summary?empSysId=1001
    [HttpGet("summary")]
    [ProducesResponseType(typeof(IEnumerable<ReimbursementSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary([FromQuery] long? empSysId = null, CancellationToken ct = default)
        => Ok(await mediator.Send(new GetReimbursementSummaryQuery(empSysId), ct));

    // POST api/reimbursements
    [HttpPost]
    [ProducesResponseType(typeof(ReimbursementDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateReimbursementRequestDto dto, CancellationToken ct = default)
    {
        var createdBy = GetCurrentUserId();
        var command = new CreateReimbursementCommand(
            dto.EmpSysId, dto.ReimType, dto.Amount, dto.Currency,
            dto.ReimDate, dto.ExpenseDate, dto.Description, dto.Location, createdBy);
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ReimId }, result);
    }

    // PUT api/reimbursements/{id}
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ReimbursementDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateReimbursementRequestDto dto, CancellationToken ct = default)
    {
        var updatedBy = GetCurrentUserId();
        var result = await mediator.Send(new UpdateReimbursementCommand(
            id, dto.ReimType, dto.Amount, dto.Currency,
            dto.ReimDate, dto.ExpenseDate, dto.Description, dto.Location, updatedBy), ct);
        return Ok(result);
    }

    // POST api/reimbursements/{id}/submit
    [HttpPost("{id:long}/submit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Submit(long id, CancellationToken ct = default)
    {
        await mediator.Send(new SubmitReimbursementCommand(id), ct);
        return NoContent();
    }

    // POST api/reimbursements/{id}/approve
    [HttpPost("{id:long}/approve")]
    [Authorize(Roles = "Approver,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Approve(long id, [FromBody] ApproveReimbursementRequestDto dto, CancellationToken ct = default)
    {
        await mediator.Send(new ApproveReimbursementCommand(id, dto.ApprovedBy, dto.ApprovalLevel), ct);
        return NoContent();
    }

    // POST api/reimbursements/{id}/reject
    [HttpPost("{id:long}/reject")]
    [Authorize(Roles = "Approver,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Reject(long id, [FromBody] RejectReimbursementRequestDto dto, CancellationToken ct = default)
    {
        await mediator.Send(new RejectReimbursementCommand(id, dto.RejectedBy, dto.Reason), ct);
        return NoContent();
    }

    // POST api/reimbursements/{id}/pay
    [HttpPost("{id:long}/pay")]
    [Authorize(Roles = "Finance,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> MarkAsPaid(long id, [FromBody] MarkAsPaidRequestDto dto, CancellationToken ct = default)
    {
        await mediator.Send(new MarkAsPaidCommand(id, dto.PaymentDate, dto.UpdatedBy), ct);
        return NoContent();
    }

    private long GetCurrentUserId()
    {
        var claim = User.FindFirst("sub") ?? User.FindFirst("nameid");
        return claim is not null && long.TryParse(claim.Value, out var id) ? id : 0;
    }
}
