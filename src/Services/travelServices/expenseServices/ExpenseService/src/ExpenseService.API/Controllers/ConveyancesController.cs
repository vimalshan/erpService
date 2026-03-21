using ExpenseService.Application.Commands;
using ExpenseService.Application.DTOs;
using ExpenseService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConveyancesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConveyancesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get conveyances for a travel request
    /// </summary>
    [HttpGet("request/{requestNumber}")]
    [ProducesResponseType(typeof(IReadOnlyList<ConveyanceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRequest(long requestNumber)
    {
        var result = await _mediator.Send(new GetConveyancesByRequestQuery { RequestNumber = requestNumber });
        return Ok(result);
    }

    /// <summary>
    /// Create a new conveyance entry
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ConveyanceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateConveyanceCommand command)
    {
        var result = await _mediator.Send(command);
        return Created($"api/conveyances/request/{result.RequestNumber}", result);
    }
}
