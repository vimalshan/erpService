using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthorizationService.Application.Commands;
using AuthorizationService.Application.DTOs;
using AuthorizationService.Application.Queries;

namespace AuthorizationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrackerRightsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TrackerRightsController> _logger;

    public TrackerRightsController(IMediator mediator, ILogger<TrackerRightsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreateTrackerRight(CreateTrackerRightCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTrackerRightsByUserId), new { userId = command.UserId }, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tracker right");
            return BadRequest("Failed to create tracker right");
        }
    }

    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TrackerRightDto>>> GetTrackerRightsByUserId(string userId)
    {
        var query = new GetTrackerRightsByUserIdQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TrackerRightDto>>> GetAllTrackerRights()
    {
        var query = new GetAllTrackerRightsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
