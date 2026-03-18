using LoanService.Application.Common;
using LoanService.Application.DTOs;
using LoanService.Application.Loans.Commands;
using LoanService.Application.Loans.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoansController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoansController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{loanNo:long}")]
    [ProducesResponseType(typeof(LoanDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(long loanNo, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLoanByIdQuery(loanNo), ct);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.Error);
    }

    [HttpGet("member/{memberId:long}")]
    [ProducesResponseType(typeof(IReadOnlyList<LoanDto>), 200)]
    public async Task<IActionResult> GetByMember(long memberId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLoansByMemberQuery(memberId), ct);
        return Ok(result.Data);
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(IReadOnlyList<LoanDto>), 200)]
    public async Task<IActionResult> GetActive(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetActiveLoansQuery(), ct);
        return Ok(result.Data);
    }

    [HttpGet("active/summary")]
    [ProducesResponseType(typeof(IEnumerable<ActiveLoanDto>), 200)]
    public async Task<IActionResult> GetActiveSummary(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetActiveLoansSummaryQuery(), ct);
        return Ok(result.Data);
    }

    [HttpPost]
    [ProducesResponseType(typeof(LoanDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateLoanCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { loanNo = result.Data!.LoanNo }, result.Data)
            : BadRequest(result.Error);
    }

    [HttpPut("{loanNo:long}/approve")]
    [ProducesResponseType(typeof(LoanDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Approve(long loanNo, [FromBody] DateTime approvalDate, CancellationToken ct)
    {
        var result = await _mediator.Send(new ApproveLoanCommand { LoanNo = loanNo, ApprovalDate = approvalDate }, ct);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.Error);
    }

    [HttpPut("{loanNo:long}/close")]
    [ProducesResponseType(typeof(LoanDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Close(long loanNo, [FromBody] DateTime closureDate, CancellationToken ct)
    {
        var result = await _mediator.Send(new CloseLoanCommand { LoanNo = loanNo, ClosureDate = closureDate }, ct);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.Error);
    }

    [HttpPost("{loanNo:long}/repayments")]
    [ProducesResponseType(typeof(RepaymentDto), 201)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AddRepayment(long loanNo, [FromBody] AddRepaymentCommand command, CancellationToken ct)
    {
        var cmd = command with { LoanNo = loanNo };
        var result = await _mediator.Send(cmd, ct);
        return result.IsSuccess ? Created(string.Empty, result.Data) : NotFound(result.Error);
    }

    [HttpPost("{loanNo:long}/repayments/{repaymentId:long}/pay")]
    [ProducesResponseType(typeof(RepaymentDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> MakePayment(long loanNo, long repaymentId, [FromBody] MakePaymentCommand command, CancellationToken ct)
    {
        var cmd = command with { LoanNo = loanNo, RepaymentId = repaymentId };
        var result = await _mediator.Send(cmd, ct);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.Error);
    }

    [HttpPost("{loanNo:long}/deductions")]
    [ProducesResponseType(typeof(DeductionDto), 201)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AddDeduction(long loanNo, [FromBody] AddDeductionCommand command, CancellationToken ct)
    {
        var cmd = command with { LoanNo = loanNo };
        var result = await _mediator.Send(cmd, ct);
        return result.IsSuccess ? Created(string.Empty, result.Data) : NotFound(result.Error);
    }
}
