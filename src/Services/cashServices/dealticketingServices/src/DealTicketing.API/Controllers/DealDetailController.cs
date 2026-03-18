using DealTicketing.Application.Features.DealDetails.Commands;
using DealTicketing.Application.Features.DealDetails.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealTicketing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class DealDetailController(IMediator mediator) : ControllerBase
{
    /// <summary>Get a deal detail by ID.</summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDealDetailByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get all deal details within a batch.</summary>
    [HttpGet("by-batch/{batchId:long}")]
    public async Task<IActionResult> GetByBatch(long batchId, CancellationToken ct)
        => Ok(await mediator.Send(new GetDealDetailsByBatchQuery(batchId), ct));

    /// <summary>Get all deals pending approval.</summary>
    [HttpGet("pending-approvals")]
    [Authorize(Roles = "DealApprover,Admin")]
    public async Task<IActionResult> GetPendingApprovals(CancellationToken ct)
        => Ok(await mediator.Send(new GetPendingApprovalsQuery(), ct));

    /// <summary>Create a deal detail within a batch.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDealDetailCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.DealId }, result);
    }

    /// <summary>Approve a deal.</summary>
    [HttpPost("{id:long}/approve")]
    [Authorize(Roles = "DealApprover,Admin")]
    public async Task<IActionResult> Approve(long id, [FromBody] ApproveDealRequest req, CancellationToken ct)
    {
        await mediator.Send(new ApproveDealCommand(id, req.AppBusiness, req.Remarks, req.ModifiedBy), ct);
        return NoContent();
    }

    /// <summary>Reject a deal.</summary>
    [HttpPost("{id:long}/reject")]
    [Authorize(Roles = "DealApprover,Admin")]
    public async Task<IActionResult> Reject(long id, [FromBody] RejectDealRequest req, CancellationToken ct)
    {
        await mediator.Send(new RejectDealCommand(id, req.Remarks, req.ModifiedBy), ct);
        return NoContent();
    }

    /// <summary>Get all settlements for a deal.</summary>
    [HttpGet("{id:long}/settlements")]
    public async Task<IActionResult> GetSettlements(long id, CancellationToken ct)
        => Ok(await mediator.Send(new GetDealSettlementsByDealQuery(id), ct));

    /// <summary>Create a settlement for a deal.</summary>
    [HttpPost("{id:long}/settlements")]
    [Authorize(Roles = "DealSettler,Admin")]
    public async Task<IActionResult> CreateSettlement(
        long id, [FromBody] CreateDealSettlementCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { DealId = id }, ct);
        return Created($"api/dealdetail/{id}/settlements/{result.SetId}", result);
    }
}

public record ApproveDealRequest(long AppBusiness, string? Remarks, decimal ModifiedBy);
public record RejectDealRequest(string Remarks, decimal ModifiedBy);
