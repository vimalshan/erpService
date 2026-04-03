using LoanTransaction.Application.Commands;
using LoanTransaction.Application.DTOs;
using LoanTransaction.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanTransaction.API.Controllers;

[ApiController]
[Route("api/v1/emi")]
[Authorize]
public class EmiController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmiController(IMediator mediator) => _mediator = mediator;

    /// <summary>Calculate EMI for given parameters.</summary>
    [HttpPost("calculate")]
    [ProducesResponseType(typeof(EmiCalculationResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Calculate([FromBody] EmiCalculationRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new CalculateEmiQuery(req.Principal, req.AnnualInterestRate, req.TenureMonths), ct);
        return Ok(result);
    }

    /// <summary>Set employee-specific interest rate.</summary>
    [HttpPost("employee-rate")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SetEmployeeRate([FromBody] SetEmployeeInterestRateCommand cmd, CancellationToken ct)
    {
        await _mediator.Send(cmd, ct);
        return NoContent();
    }
}

public record EmiCalculationRequest(decimal Principal, int AnnualInterestRate, int TenureMonths);
