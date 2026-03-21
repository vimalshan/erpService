using FinanceService.Application.DTOs;
using FinanceService.Application.Features.Batches.Commands.AddBatchLineItem;
using FinanceService.Application.Features.Batches.Commands.ApproveBatch;
using FinanceService.Application.Features.Batches.Commands.CreateBatch;
using FinanceService.Application.Features.Batches.Queries.GetAllBatches;
using FinanceService.Application.Features.Batches.Queries.GetBatchById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BatchesController : ControllerBase
{
    private readonly IMediator _mediator;

    public BatchesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<BatchDto>>> GetAll([FromQuery] string? unitCode, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllBatchesQuery { UnitCode = unitCode }, ct);
        return Ok(result);
    }

    [HttpGet("{unitCode}/{batchNumber:decimal}")]
    public async Task<ActionResult<BatchDto>> GetById(string unitCode, decimal batchNumber, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBatchByIdQuery(unitCode, batchNumber), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BatchDto>> Create([FromBody] CreateBatchCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { unitCode = result.UnitCode, batchNumber = result.BatchNumber }, result);
    }

    [HttpPost("{unitCode}/{batchNumber:decimal}/lines")]
    public async Task<IActionResult> AddLineItem(string unitCode, decimal batchNumber, [FromBody] AddBatchLineItemCommand command, CancellationToken ct)
    {
        if (unitCode != command.UnitCode || batchNumber != command.BatchNumber)
            return BadRequest("Unit code or batch number mismatch.");
        await _mediator.Send(command, ct);
        return Ok(new { message = "Line item added successfully." });
    }

    [HttpPost("{unitCode}/{batchNumber:decimal}/approve")]
    public async Task<IActionResult> Approve(string unitCode, decimal batchNumber, [FromBody] ApproveBatchCommand command, CancellationToken ct)
    {
        if (unitCode != command.UnitCode || batchNumber != command.BatchNumber)
            return BadRequest("Unit code or batch number mismatch.");
        await _mediator.Send(command, ct);
        return Ok(new { message = "Batch approved successfully." });
    }
}
