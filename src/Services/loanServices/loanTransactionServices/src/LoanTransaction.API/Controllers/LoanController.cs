using LoanTransaction.Application.Commands;
using LoanTransaction.Application.DTOs;
using LoanTransaction.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanTransaction.API.Controllers;

[ApiController]
[Route("api/v1/loans")]
[Authorize]
public class LoanController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoanController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get a loan by loan number.</summary>
    [HttpGet("{loanNo:long}")]
    [ProducesResponseType(typeof(LoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long loanNo, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLoanByIdQuery(loanNo), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get all loans for an employee.</summary>
    [HttpGet("employee/{empId}")]
    [ProducesResponseType(typeof(IEnumerable<LoanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(int empId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLoansByEmployeeQuery(empId), ct);
        return Ok(result);
    }

    /// <summary>Get all loans (paged).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedLoanResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllLoansQuery(page, pageSize), ct);
        return Ok(result);
    }

    /// <summary>Disburse a new loan.</summary>
    [HttpPost("disburse")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Disburse([FromBody] DisburseLoanDto dto, CancellationToken ct)
    {
        var cmd = new DisburseLoanCommand(
            dto.ApplicationId, dto.EmployeeId, dto.LoanDefinitionId,
            dto.GradeId, dto.UnitId, dto.SubclassId, dto.GuarantorId,
            dto.DisbursementType, dto.PrincipalAmount, dto.InterestRate,
            dto.TenureMonths, dto.RecoveryMethod, dto.EffectiveDate,
            dto.FirstInstallmentDate, dto.Reason, dto.CompoundingFactor,
            dto.InterestFrequency, dto.HasEmployeeInterestRate,
            dto.AmountEdId, dto.PrnEdId, dto.IntEdId, dto.CreatedBy);

        var loanNo = await _mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { loanNo }, new { loanNo });
    }

    /// <summary>Close a loan.</summary>
    [HttpPost("{loanNo:long}/close")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CloseLoan(long loanNo, [FromBody] CloseLoanDto dto, CancellationToken ct)
    {
        await _mediator.Send(new CloseLoanCommand(loanNo, dto.ClosureType, dto.ClosedBy), ct);
        return NoContent();
    }

    /// <summary>Adjust a loan.</summary>
    [HttpPost("{loanNo:long}/adjust")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AdjustLoan(long loanNo, [FromBody] AdjustLoanDto dto, CancellationToken ct)
    {
        await _mediator.Send(new AdjustLoanCommand(loanNo, dto.AdjLoanNo, dto.AdjPrincipalAmount, dto.AdjInterestAmount, dto.UpdatedBy), ct);
        return NoContent();
    }
}
