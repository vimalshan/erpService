using LoanTransaction.Application.DTOs;
using LoanTransaction.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanTransaction.API.Controllers;

[ApiController]
[Route("api/v1/ledger")]
[Authorize]
public class LedgerController : ControllerBase
{
    private readonly IMediator _mediator;

    public LedgerController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get ledger entries for a loan.</summary>
    [HttpGet("{loanNo:long}")]
    [ProducesResponseType(typeof(IEnumerable<LoanLedgerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByLoan(long loanNo, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLoanLedgerQuery(loanNo), ct);
        return Ok(result);
    }

    /// <summary>Get ledger entries for an employee.</summary>
    [HttpGet("employee/{empId}")]
    [ProducesResponseType(typeof(IEnumerable<LoanLedgerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(int empId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLoanLedgerByEmployeeQuery(empId), ct);
        return Ok(result);
    }
}
