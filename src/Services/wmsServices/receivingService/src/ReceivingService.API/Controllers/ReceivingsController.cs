using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReceivingService.Application.Commands.CancelReceiving;
using ReceivingService.Application.Commands.CloseReceiving;
using ReceivingService.Application.Commands.CreateReceiving;
using ReceivingService.Application.DTOs;
using ReceivingService.Application.Queries.GetAllReceivings;
using ReceivingService.Application.Queries.GetReceivingById;

namespace ReceivingService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public sealed class ReceivingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReceivingsController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>List all receivings (paged).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReceivingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllReceivingsQuery(page, pageSize), ct);
        return Ok(result);
    }

    /// <summary>Get a receiving by its ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReceivingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetReceivingByIdQuery(id), ct);
        return Ok(result);
    }

    /// <summary>Create a new receiving against a Purchase Order.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReceivingDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateReceivingCommand command,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Close a receiving (no further lines can be added).</summary>
    [HttpPut("{id:int}/close")]
    [ProducesResponseType(typeof(ReceivingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Close(int id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new CloseReceivingCommand(id), ct);
        return Ok(result);
    }

    /// <summary>Cancel a receiving.</summary>
    [HttpPut("{id:int}/cancel")]
    [ProducesResponseType(typeof(ReceivingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new CancelReceivingCommand(id), ct);
        return Ok(result);
    }
}
