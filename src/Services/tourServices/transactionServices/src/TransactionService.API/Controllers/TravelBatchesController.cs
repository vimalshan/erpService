using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.TravelBatches.Commands.ApproveTravelBatch;
using TransactionService.Application.TravelBatches.Commands.CreateTravelBatch;
using TransactionService.Application.TravelBatches.Queries;

namespace TransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class TravelBatchesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TravelBatchesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? status = null,
        [FromQuery] string? vendorId = null,
        CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetAllTravelBatchesQuery(page, pageSize, status, vendorId), ct));

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTravelBatchByIdQuery(id), ct));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTravelBatchCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.BatchId }, result);
    }

    [HttpPatch("{id}/admin-approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdminApprove(
        string id,
        [FromQuery] string approvedBy,
        [FromQuery] string? approvedAmount = null,
        [FromQuery] string? remarks = null,
        CancellationToken ct = default)
    {
        await _mediator.Send(new AdminApproveTravelBatchCommand(id, approvedBy, approvedAmount, remarks), ct);
        return NoContent();
    }

    [HttpPatch("{id}/finance-approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FinanceApprove(
        string id,
        [FromQuery] string approvedBy,
        [FromQuery] string? remarks = null,
        CancellationToken ct = default)
    {
        await _mediator.Send(new FinanceApproveTravelBatchCommand(id, approvedBy, remarks), ct);
        return NoContent();
    }

    [HttpPatch("{id}/post-jv")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostJV(string id, [FromQuery] string jvId, CancellationToken ct)
    {
        await _mediator.Send(new PostTravelBatchJVCommand(id, jvId), ct);
        return NoContent();
    }

    [HttpPatch("{id}/reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(
        string id,
        [FromQuery] string rejectedBy,
        [FromQuery] string? remarks = null,
        CancellationToken ct = default)
    {
        await _mediator.Send(new RejectTravelBatchCommand(id, rejectedBy, remarks), ct);
        return NoContent();
    }
}
