using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StipendService.Application.DTOs;
using StipendService.Application.Features.StipendDisbursement.Commands;
using StipendService.Application.Features.StipendDisbursement.Queries;
using StipendService.Infrastructure.Dapper;

namespace StipendService.API.Controllers;

[ApiController]
[Route("api/v1/disbursement")]
[Authorize]
public class StipendDisbursementController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly StipendDapperRepository _dapperRepository;

    public StipendDisbursementController(IMediator mediator, StipendDapperRepository dapperRepository)
    {
        _mediator = mediator;
        _dapperRepository = dapperRepository;
    }

    /// <summary>Gets disbursement record by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(StipendDisbursementDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDisbursementByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Gets all disbursements for a specific month (YYYY-MM).</summary>
    [HttpGet("by-month/{monthYear}")]
    [ProducesResponseType(typeof(IEnumerable<StipendDisbursementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByMonth(string monthYear, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDisbursementsByMonthQuery(monthYear), cancellationToken);
        return Ok(result);
    }

    /// <summary>Gets all disbursements for an SRF member.</summary>
    [HttpGet("by-srf/{srfId:long}")]
    [ProducesResponseType(typeof(IEnumerable<StipendDisbursementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySrf(long srfId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDisbursementsBySrfQuery(srfId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Calculates and creates disbursement records for a month.</summary>
    [HttpPost("calculate")]
    [ProducesResponseType(typeof(CalculateDisbursementResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Calculate([FromBody] CalculateAndDisburseStipendCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>Processes draft disbursements (marks them as Processed).</summary>
    [HttpPost("process")]
    [ProducesResponseType(typeof(ProcessMonthlyStipendResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Process([FromBody] ProcessMonthlyStipendCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>Processes disbursements via stored procedure (Dapper).</summary>
    [HttpPost("process-sp")]
    [ProducesResponseType(typeof(ProcessMonthlyStipendResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ProcessViaSP([FromQuery] string monthYear, [FromQuery] long processedBy)
    {
        var result = await _dapperRepository.ProcessMonthlyStipendAsync(monthYear, processedBy);
        return Ok(result);
    }

    /// <summary>Calculates disbursements via stored procedure (Dapper).</summary>
    [HttpPost("calculate-sp")]
    [ProducesResponseType(typeof(CalculateDisbursementResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CalculateViaSP([FromQuery] string monthYear, [FromQuery] long processedBy)
    {
        var result = await _dapperRepository.CalculateAndDisburseAsync(monthYear, processedBy);
        return Ok(result);
    }

    /// <summary>Rejects a disbursement record.</summary>
    [HttpPost("{id:long}/reject")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Reject(long id, [FromQuery] long updatedBy, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RejectDisbursementCommand(id, updatedBy), cancellationToken);
        return Ok(result);
    }

    /// <summary>Sets bank reference for a disbursement.</summary>
    [HttpPut("{id:long}/bank-reference")]
    [ProducesResponseType(typeof(StipendDisbursementDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SetBankReference(long id, [FromBody] SetBankReferenceCommand command, CancellationToken cancellationToken)
    {
        if (id != command.DisbursementId) return BadRequest("ID mismatch.");
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
