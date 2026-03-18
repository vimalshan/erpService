using DeductionService.Application.CQRS.Commands.CancelDeduction;
using DeductionService.Application.CQRS.Commands.CreateAdhocDeduction;
using DeductionService.Application.CQRS.Commands.ProcessMonthlyDeduction;
using DeductionService.Application.CQRS.Queries.GetDeductionAmount;
using DeductionService.Application.CQRS.Queries.GetDeductionById;
using DeductionService.Application.CQRS.Queries.GetDeductionHistory;
using DeductionService.Application.CQRS.Queries.GetDeductionsByEmployee;
using DeductionService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeductionService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DeductionController(IMediator mediator) : ControllerBase
{
    /// <summary>Gets a deduction by system ID.</summary>
    [HttpGet("{systemId:long}")]
    [ProducesResponseType(typeof(AdhocPayDeductionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long systemId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDeductionByIdQuery(systemId), ct);
        return Ok(result);
    }

    /// <summary>Gets all deductions for an employee.</summary>
    [HttpGet("employee/{employeeNumber:long}")]
    [ProducesResponseType(typeof(IEnumerable<AdhocPayDeductionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long employeeNumber, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDeductionsByEmployeeQuery(employeeNumber), ct);
        return Ok(result);
    }

    /// <summary>Gets deduction history for an employee.</summary>
    [HttpGet("employee/{employeeNumber:long}/history")]
    [ProducesResponseType(typeof(IEnumerable<AdhocPayDeductionHistoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory(long employeeNumber, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDeductionHistoryQuery(employeeNumber), ct);
        return Ok(result);
    }

    /// <summary>Gets deduction amount from the canteen pricing function.</summary>
    [HttpGet("amount")]
    [ProducesResponseType(typeof(DeductionAmountDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAmount(
        [FromQuery] long empSysId,
        [FromQuery] long itemCode,
        [FromQuery] DateTime dateTaken,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetDeductionAmountQuery(empSysId, itemCode, dateTaken), ct);
        return Ok(result);
    }

    /// <summary>Creates an ad-hoc payroll deduction.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(AdhocPayDeductionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAdhocDeductionDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateAdhocDeductionCommand(
            dto.SystemId, dto.CanteenUnit, dto.PayAmount, dto.EarningDeductionCode,
            dto.EmployeeNumber, dto.EnteredByUserId, dto.CompanyCode, dto.GradeType), ct);

        return CreatedAtAction(nameof(GetById), new { systemId = result.SystemId }, result);
    }

    /// <summary>Cancels a deduction by system ID.</summary>
    [HttpDelete("{systemId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(long systemId, [FromQuery] long cancelledByUserId, CancellationToken ct)
    {
        await mediator.Send(new CancelDeductionCommand(systemId, cancelledByUserId), ct);
        return NoContent();
    }

    /// <summary>Processes monthly canteen deductions for payroll.</summary>
    [HttpPost("process-monthly")]
    [Authorize(Roles = "PayrollAdmin")]
    [ProducesResponseType(typeof(ProcessMonthlyDeductionResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessMonthly(
        [FromBody] ProcessMonthlyDeductionCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }
}
