using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BatchService.Application.Commands.CloseBatch;
using BatchService.Application.Commands.CreateBatch;
using BatchService.Application.Commands.DeleteBatch;
using BatchService.Application.Commands.UpdateBatch;
using BatchService.Application.DTOs;
using BatchService.Application.Queries.GetAllBatches;
using BatchService.Application.Queries.GetBatch;
using BatchService.Application.Queries.GetBatchesByMonth;

namespace BatchService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class BatchController : ControllerBase
{
    private readonly IMediator _mediator;

    public BatchController(IMediator mediator) => _mediator = mediator;

    // GET api/batch
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BatchDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetAllBatchesQuery(), ct));

    // GET api/batch/{id}
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(BatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBatchQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    // GET api/batch/month/{monthNo}
    [HttpGet("month/{monthNo:int}")]
    [ProducesResponseType(typeof(IEnumerable<BatchDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByMonth(int monthNo, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetBatchesByMonthQuery(monthNo), ct));

    // POST api/batch
    [HttpPost]
    [ProducesResponseType(typeof(BatchDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBatchRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateBatchCommand(req.BatchId, req.MonthNo, req.ModifiedBy), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.BatchId }, result);
    }

    // PUT api/batch/{id}
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(BatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateBatchRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateBatchCommand(id, req.MonthNo, req.ModifiedBy), ct);
        return Ok(result);
    }

    // POST api/batch/{id}/close
    [HttpPost("{id:long}/close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Close(long id, [FromQuery] long modifiedBy, CancellationToken ct)
    {
        await _mediator.Send(new CloseBatchCommand(id, modifiedBy), ct);
        return NoContent();
    }

    // DELETE api/batch/{id}
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteBatchCommand(id), ct);
        return NoContent();
    }
}
