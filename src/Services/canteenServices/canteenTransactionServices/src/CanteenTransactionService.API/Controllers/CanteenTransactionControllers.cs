using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CanteenTransactionService.Application.CQRS.Commands;
using CanteenTransactionService.Application.CQRS.Queries;
using CanteenTransactionService.Application.DTOs;

namespace CanteenTransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CanteenTransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public CanteenTransactionController(IMediator mediator) => _mediator = mediator;

    /// <summary>Record a new canteen meal transaction.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CanteenDaconDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RecordTransaction([FromBody] RecordCanteenTransactionCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetBySerialNumber), new { serialNumber = result.SerialNumber }, result);
    }

    /// <summary>Get transaction by serial number.</summary>
    [HttpGet("{serialNumber:long}")]
    [ProducesResponseType(typeof(CanteenDaconDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetBySerialNumber(long serialNumber, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTransactionBySerialNumberQuery(serialNumber), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get transactions by employee within date range.</summary>
    [HttpGet("employee/{employeeSysId:long}")]
    [ProducesResponseType(typeof(IEnumerable<CanteenDaconDto>), 200)]
    public async Task<IActionResult> GetByEmployee(long employeeSysId, [FromQuery] string fromDate, [FromQuery] string toDate, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTransactionsByEmployeeQuery(employeeSysId, fromDate, toDate), ct);
        return Ok(result);
    }

    /// <summary>Get transactions by company and date.</summary>
    [HttpGet("company/{companyCode:long}")]
    [ProducesResponseType(typeof(IEnumerable<CanteenDaconDto>), 200)]
    public async Task<IActionResult> GetByCompanyAndDate(long companyCode, [FromQuery] string swipeDate, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTransactionsByCompanyAndDateQuery(companyCode, swipeDate), ct);
        return Ok(result);
    }

    /// <summary>Cancel a canteen transaction.</summary>
    [HttpDelete("{serialNumber:long}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CancelTransaction(long serialNumber, CancellationToken ct)
    {
        await _mediator.Send(new CancelCanteenTransactionCommand(serialNumber), ct);
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class DailyAvailedController : ControllerBase
{
    private readonly IMediator _mediator;

    public DailyAvailedController(IMediator mediator) => _mediator = mediator;

    /// <summary>Process a daily availed record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(DailyAvailedDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> ProcessDailyAvailed([FromBody] ProcessDailyAvailedCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetBySerialNumber), new { serialNumber = result.SerialNumber }, result);
    }

    /// <summary>Get daily availed record by serial number.</summary>
    [HttpGet("{serialNumber:long}")]
    [ProducesResponseType(typeof(DailyAvailedDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetBySerialNumber(long serialNumber, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDailyAvailedBySerialNumberQuery(serialNumber), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get daily availed records by employee within date range.</summary>
    [HttpGet("employee/{employeeSysId:long}")]
    [ProducesResponseType(typeof(IEnumerable<DailyAvailedDto>), 200)]
    public async Task<IActionResult> GetByEmployee(long employeeSysId, [FromQuery] string fromDate, [FromQuery] string toDate, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDailyAvailedByEmployeeQuery(employeeSysId, fromDate, toDate), ct);
        return Ok(result);
    }

    /// <summary>Get daily availed records by company and date.</summary>
    [HttpGet("company/{companyCode:long}")]
    [ProducesResponseType(typeof(IEnumerable<DailyAvailedDto>), 200)]
    public async Task<IActionResult> GetByCompanyAndDate(long companyCode, [FromQuery] string swipeDate, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDailyAvailedByCompanyAndDateQuery(companyCode, swipeDate), ct);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class MisBatchController : ControllerBase
{
    private readonly IMediator _mediator;

    public MisBatchController(IMediator mediator) => _mediator = mediator;

    /// <summary>Submit a new MIS batch record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(MisBatchSubmissionDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> SubmitBatch([FromBody] SubmitMisBatchCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetBySerialNumber), new { serialNumber = result.SerialNumber }, result);
    }

    /// <summary>Get MIS batch by serial number.</summary>
    [HttpGet("{serialNumber:long}")]
    [ProducesResponseType(typeof(MisBatchSubmissionDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetBySerialNumber(long serialNumber, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMisBatchBySerialNumberQuery(serialNumber), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get MIS batch records by batch number.</summary>
    [HttpGet("batch/{batchNumber:long}")]
    [ProducesResponseType(typeof(IEnumerable<MisBatchSubmissionDto>), 200)]
    public async Task<IActionResult> GetByBatchNumber(long batchNumber, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMisBatchByBatchNumberQuery(batchNumber), ct);
        return Ok(result);
    }

    /// <summary>Get all pending MIS batches.</summary>
    [HttpGet("pending")]
    [ProducesResponseType(typeof(IEnumerable<MisBatchSubmissionDto>), 200)]
    public async Task<IActionResult> GetPending(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPendingMisBatchesQuery(), ct);
        return Ok(result);
    }

    /// <summary>Mark MIS batch as processed.</summary>
    [HttpPatch("{serialNumber:long}/process")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ProcessBatch(long serialNumber, CancellationToken ct)
    {
        await _mediator.Send(new ProcessMisBatchCommand(serialNumber), ct);
        return NoContent();
    }

    /// <summary>Mark MIS batch as failed.</summary>
    [HttpPatch("{serialNumber:long}/fail")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> FailBatch(long serialNumber, CancellationToken ct)
    {
        await _mediator.Send(new FailMisBatchCommand(serialNumber), ct);
        return NoContent();
    }
}
