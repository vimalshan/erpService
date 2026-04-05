using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.DTOs;
using TransactionService.Application.Features.TransactionLogs.Commands;
using TransactionService.Application.Features.TransactionLogs.Queries;
using TransactionService.Application.Features.StoredProcedures.Commands;
using TransactionService.Application.Features.StoredProcedures.Queries;

namespace TransactionService.API.Controllers;

[ApiController]
[Route("api/v1/transaction-logs")]
[Authorize]
public class TransactionLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionLogsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TransactionLogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTransactionLogsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TransactionLogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTransactionLogByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-entity")]
    [ProducesResponseType(typeof(IEnumerable<TransactionLogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEntity([FromQuery] string transactionType, [FromQuery] long transactionId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTransactionLogsByEntityQuery(transactionType, transactionId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("by-action/{action}")]
    [ProducesResponseType(typeof(IEnumerable<TransactionLogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByAction(string action, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTransactionLogsByActionQuery(action), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TransactionLogDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Log([FromBody] LogTransactionCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.LogId }, result);
    }

    // --- Stored Procedure Endpoints ---

    [HttpGet("sp/pending-approvals")]
    [ProducesResponseType(typeof(StoredProcResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingApprovals([FromQuery] long? approverId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPendingApprovalsSpQuery(approverId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("sp/audit-log")]
    [ProducesResponseType(typeof(StoredProcResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAuditLog([FromQuery] string? entityType, [FromQuery] long? entityId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAuditLogSpQuery(entityType, entityId, fromDate, toDate), cancellationToken);
        return Ok(result);
    }

    [HttpGet("sp/pending-disbursements")]
    [ProducesResponseType(typeof(StoredProcResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingDisbursements(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPendingDisbursementsSpQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("sp/available-rooms")]
    [ProducesResponseType(typeof(StoredProcResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime date, [FromQuery] TimeSpan startTime, [FromQuery] TimeSpan endTime, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAvailableRoomsSpQuery(date, startTime, endTime), cancellationToken);
        return Ok(result);
    }

    [HttpGet("sp/validate-attendees/{bookingId:long}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateAttendees(long bookingId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ValidateBookingAttendeesSpQuery(bookingId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("sp/calculate-stipend")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public async Task<IActionResult> CalculateStipend([FromQuery] long researchCategoryId, [FromQuery] long rankId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CalculateStipendSpQuery(researchCategoryId, rankId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("sp/process-monthly-stipend")]
    [ProducesResponseType(typeof(StoredProcResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ProcessMonthlyStipend([FromBody] ProcessMonthlyStipendSpCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
