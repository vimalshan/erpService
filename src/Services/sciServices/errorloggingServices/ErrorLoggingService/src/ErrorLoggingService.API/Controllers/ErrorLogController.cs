using ErrorLoggingService.Application.Commands.LogError;
using ErrorLoggingService.Application.Queries.GetErrorLogs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErrorLoggingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ErrorLogController : ControllerBase
{
    private readonly IMediator _mediator;

    public ErrorLogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Logs an error entry.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LogError([FromBody] LogErrorCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetErrorLogs), new { }, new { id });
    }

    /// <summary>Retrieves error logs within a date range.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetErrorLogs(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetErrorLogsQuery(startDate, endDate), cancellationToken);
        return Ok(result);
    }
}
