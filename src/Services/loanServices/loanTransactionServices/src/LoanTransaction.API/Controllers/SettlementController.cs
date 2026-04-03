using LoanTransaction.Application.DTOs;
using LoanTransaction.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanTransaction.API.Controllers;

[ApiController]
[Route("api/v1/settlements")]
[Authorize]
public class SettlementController : ControllerBase
{
    private readonly IMediator _mediator;

    public SettlementController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get settlements for a loan.</summary>
    [HttpGet("{loanNo:long}")]
    [ProducesResponseType(typeof(IEnumerable<LoanSettlementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByLoan(long loanNo, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLoanSettlementsQuery(loanNo), ct);
        return Ok(result);
    }
}
