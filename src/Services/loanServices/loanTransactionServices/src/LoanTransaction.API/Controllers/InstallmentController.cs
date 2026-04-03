using LoanTransaction.Application.Commands;
using LoanTransaction.Application.DTOs;
using LoanTransaction.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanTransaction.API.Controllers;

[ApiController]
[Route("api/v1/installments")]
[Authorize]
public class InstallmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public InstallmentController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get full EMI schedule for a loan.</summary>
    [HttpGet("{loanNo:long}/schedule")]
    [ProducesResponseType(typeof(IEnumerable<LoanInstallmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSchedule(long loanNo, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetInstallmentScheduleQuery(loanNo), ct);
        return Ok(result);
    }

    /// <summary>Get pending installments for a loan.</summary>
    [HttpGet("{loanNo:long}/pending")]
    [ProducesResponseType(typeof(IEnumerable<LoanInstallmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending(long loanNo, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPendingInstallmentsQuery(loanNo), ct);
        return Ok(result);
    }

    /// <summary>Record an EMI payment.</summary>
    [HttpPost("payment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RecordPayment([FromBody] RecordEmiPaymentDto dto, CancellationToken ct)
    {
        await _mediator.Send(new RecordEmiPaymentCommand(
            dto.LoanNo, dto.InstallmentId, dto.PrincipalPaid, dto.InterestPaid, dto.PaidBy), ct);
        return NoContent();
    }

    /// <summary>Create EMI schedule for an existing loan.</summary>
    [HttpPost("{loanNo:long}/schedule")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(IEnumerable<EmiScheduleItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateSchedule(long loanNo, [FromBody] CreateEmiScheduleRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateEmiScheduleCommand(
            loanNo, req.EmployeeId, req.UnitId, req.PrincipalAmount,
            req.InterestRate, req.TenureMonths, req.FirstInstallmentDate, req.CreatedBy), ct);
        return Ok(result);
    }
}

public record CreateEmiScheduleRequest(
    long EmployeeId,
    long UnitId,
    decimal PrincipalAmount,
    int InterestRate,
    int TenureMonths,
    DateTime FirstInstallmentDate,
    long CreatedBy);
