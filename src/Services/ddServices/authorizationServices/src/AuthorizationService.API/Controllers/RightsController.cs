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
public class RightsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RightsController> _logger;

    public RightsController(IMediator mediator, ILogger<RightsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreateRight(CreateRightCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetRight), new { id }, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating right");
            return BadRequest("Failed to create right");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RightDto>> GetRight(long id)
    {
        var query = new GetRightByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound($"Right with ID {id} not found");

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RightDto>>> GetAllRights()
    {
        var query = new GetAllRightsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
