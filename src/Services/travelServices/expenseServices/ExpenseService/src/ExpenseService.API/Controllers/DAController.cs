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
public class DAController : ControllerBase
{
    private readonly IMediator _mediator;

    public DAController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Calculate DA for a travel request
    /// </summary>
    [HttpPost("calculate")]
    [ProducesResponseType(typeof(DaSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Calculate([FromBody] CalculateDACommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Get DA summary for a request
    /// </summary>
    [HttpGet("{requestId}")]
    [ProducesResponseType(typeof(DaSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSummary(long requestId)
    {
        var result = await _mediator.Send(new GetDaSummaryQuery { RequestId = requestId });
        return result == null ? NotFound() : Ok(result);
    }
}
