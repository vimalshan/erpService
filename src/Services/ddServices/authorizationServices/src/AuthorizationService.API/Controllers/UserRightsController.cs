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
public class UserRightsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserRightsController> _logger;

    public UserRightsController(IMediator mediator, ILogger<UserRightsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreateUserRight(CreateUserRightCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserRightsByUserId), new { userId = command.UserId }, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user right");
            return BadRequest("Failed to create user right");
        }
    }

    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserRightDto>>> GetUserRightsByUserId(string userId)
    {
        var query = new GetUserRightsByUserIdQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserRightDto>>> GetAllUserRights()
    {
        var query = new GetAllUserRightsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> DeleteUserRight(long id)
    {
        var command = new DeleteUserRightCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound($"User right with ID {id} not found");

        return Ok(true);
    }
}
