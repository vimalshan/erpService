using LoanManagement.Application.Commands.AddDisbursement;
using LoanManagement.Application.Commands.AddInterest;
using LoanManagement.Application.Commands.AddRepaymentSchedule;
using LoanManagement.Application.Commands.CloseLoan;
using LoanManagement.Application.Commands.CreateLoan;
using LoanManagement.Application.DTOs;
using LoanManagement.Application.Queries.GetAllLoans;
using LoanManagement.Application.Queries.GetLoanById;
using LoanManagement.Application.Queries.GetRepaymentSchedule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoansController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LoanDto>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] decimal? orgId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllLoansQuery(orgId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{loanId:decimal}")]
    [ProducesResponseType(typeof(LoanDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(decimal loanId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLoanByIdQuery(loanId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(LoanDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateLoanCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { loanId = result.LoanId }, result);
    }

    [HttpPost("{loanId:decimal}/disbursements")]
    [ProducesResponseType(typeof(DisbursementScheduleDto), 201)]
    public async Task<IActionResult> AddDisbursement(
        decimal loanId,
        [FromBody] AddDisbursementRequest req,
        CancellationToken cancellationToken)
    {
        var cmd = new AddDisbursementCommand(loanId, req.DisbDate, req.Amount, req.ExcRate);
        var result = await _mediator.Send(cmd, cancellationToken);
        return StatusCode(201, result);
    }

    [HttpPost("{loanId:decimal}/interests")]
    [ProducesResponseType(typeof(InterestDto), 201)]
    public async Task<IActionResult> AddInterest(
        decimal loanId,
        [FromBody] AddInterestRequest req,
        CancellationToken cancellationToken)
    {
        var cmd = new AddInterestCommand(loanId, req.RateType, req.Percentage, req.FloatTypeId, req.EffectiveDate);
        var result = await _mediator.Send(cmd, cancellationToken);
        return StatusCode(201, result);
    }

    [HttpPost("{loanId:decimal}/repayments")]
    [ProducesResponseType(typeof(List<RepaymentScheduleDto>), 201)]
    public async Task<IActionResult> AddRepaymentSchedule(
        decimal loanId,
        [FromBody] AddRepaymentScheduleRequest req,
        CancellationToken cancellationToken)
    {
        var cmd = new AddRepaymentScheduleCommand(loanId, req.Lines);
        var result = await _mediator.Send(cmd, cancellationToken);
        return StatusCode(201, result);
    }

    [HttpGet("{loanId:decimal}/repayments")]
    [ProducesResponseType(typeof(IEnumerable<RepaymentScheduleDto>), 200)]
    public async Task<IActionResult> GetRepaymentSchedule(decimal loanId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRepaymentScheduleQuery(loanId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{loanId:decimal}/close")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> CloseLoan(
        decimal loanId,
        [FromBody] CloseLoanRequest req,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new CloseLoanCommand(loanId, req.ModifiedBy), cancellationToken);
        return NoContent();
    }
}

public record AddDisbursementRequest(DateTime DisbDate, decimal Amount, decimal? ExcRate);
public record AddInterestRequest(string RateType, decimal Percentage, long? FloatTypeId, DateTime EffectiveDate);
public record AddRepaymentScheduleRequest(List<RepaymentLineItem> Lines);
public record CloseLoanRequest(decimal ModifiedBy);
