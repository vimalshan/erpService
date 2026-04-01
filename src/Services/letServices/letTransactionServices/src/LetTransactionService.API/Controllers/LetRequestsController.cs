using LetTransactionService.Application.Commands.AddLetSub;
using LetTransactionService.Application.Commands.CreateLetRequest;
using LetTransactionService.Application.Commands.UpdateLetSub;
using LetTransactionService.Application.DTOs;
using LetTransactionService.Application.Queries.GetLetRequest;
using LetTransactionService.Application.Queries.GetLetRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetTransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class LetRequestsController(IMediator mediator) : ControllerBase
{
    /// <summary>Get all LET requests with optional employee filtering and pagination.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LetSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? employeeUserId = null,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetLetRequestsQuery(page, pageSize, employeeUserId), ct);
        return Ok(result);
    }

    /// <summary>Get a specific LET request by request number.</summary>
    [HttpGet("{requestNumber:long}")]
    [ProducesResponseType(typeof(LetMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long requestNumber, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLetRequestQuery(requestNumber), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new LET request.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(LetMainDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateLetRequestCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { requestNumber = result.RequestNumber }, result);
    }

    /// <summary>Add a sub-entry to an existing LET request.</summary>
    [HttpPost("{requestNumber:long}/sub")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddSub(long requestNumber, [FromBody] AddLetSubCommand command, CancellationToken ct)
    {
        if (command.RequestNumber != requestNumber)
            return BadRequest("Request number mismatch.");

        await mediator.Send(command, ct);
        return Ok(new { message = "Sub-entry added successfully." });
    }

    /// <summary>Update review data for a LET sub-entry.</summary>
    [HttpPut("{requestNumber:long}/sub/{serialNumber:int}/review")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSubReview(
        long requestNumber, int serialNumber,
        [FromBody] UpdateLetSubCommand command, CancellationToken ct)
    {
        if (command.RequestNumber != requestNumber || command.SerialNumber != serialNumber)
            return BadRequest("Route parameter mismatch.");

        await mediator.Send(command, ct);
        return Ok(new { message = "Sub-entry review updated successfully." });
    }
}
