using DealTicketing.Application.Features.DealBatches.Commands;
using DealTicketing.Application.Features.DealBatches.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealTicketing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class DealBatchController(IMediator mediator) : ControllerBase
{
    /// <summary>Get a deal batch by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDealBatchByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get deal batches by date.</summary>
    [HttpGet("by-date/{date:datetime}")]
    public async Task<IActionResult> GetByDate(DateTime date, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDealBatchesByDateQuery(date), ct);
        return Ok(result);
    }

    /// <summary>Create a new deal batch.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateDealBatchCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.DealBatchId }, result);
    }

    /// <summary>Reject a deal batch.</summary>
    [HttpPost("{id:long}/reject")]
    [Authorize(Roles = "DealApprover,Admin")]
    public async Task<IActionResult> Reject(long id, [FromBody] RejectDealBatchRequest req, CancellationToken ct)
    {
        await mediator.Send(new RejectDealBatchCommand(id, req.Reason, req.ModifiedBy), ct);
        return NoContent();
    }

    /// <summary>Update Reuters screenshot on a deal batch.</summary>
    [HttpPut("{id:long}/screenshot")]
    public async Task<IActionResult> UpdateScreenshot(long id, [FromBody] UpdateScreenshotRequest req, CancellationToken ct)
    {
        await mediator.Send(new UpdateDealBatchScreenshotCommand(id, req.Screenshot, req.ModifiedBy), ct);
        return NoContent();
    }
}

public record RejectDealBatchRequest(string Reason, decimal ModifiedBy);
public record UpdateScreenshotRequest(string Screenshot, decimal ModifiedBy);
