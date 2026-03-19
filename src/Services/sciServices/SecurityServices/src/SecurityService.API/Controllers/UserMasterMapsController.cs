using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityService.Application.Commands.Users;
using SecurityService.Application.DTOs;
using SecurityService.Application.Queries;

namespace SecurityService.API.Controllers;

[ApiController]
[Route("api/v1/usermaps")]
[Produces("application/json")]
public sealed class UserMasterMapsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserMasterMapsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all user master maps.</summary>
    [HttpGet]
    [Authorize(Policy = "SecurityManager")]
    public async Task<ActionResult<IEnumerable<UserMasterMapDto>>> GetAll(CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetUserMapsQuery(), ct));

    /// <summary>Get user maps for a specific user.</summary>
    [HttpGet("user/{userId:long}")]
    [Authorize(Policy = "SecurityManager")]
    public async Task<ActionResult<IEnumerable<UserMasterMapDto>>> GetByUser(
        long userId, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetUserMapsByUserQuery(userId), ct));

    /// <summary>Get a user map by ID.</summary>
    [HttpGet("{id:long}")]
    [Authorize(Policy = "SecurityManager")]
    public async Task<ActionResult<UserMasterMapDto>> GetById(long id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetUserMapByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new user master map entry.</summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<UserMasterMapDto>> Create(
        [FromBody] CreateUserMapCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.MapId }, result);
    }

    /// <summary>Update a user master map entry.</summary>
    [HttpPut("{id:long}")]
    [Authorize(Policy = "SecurityManager")]
    public async Task<ActionResult<UserMasterMapDto>> Update(
        long id, [FromBody] UpdateUserMapCommand command, CancellationToken ct = default)
    {
        if (id != command.MapId) return BadRequest("ID mismatch.");
        return Ok(await _mediator.Send(command, ct));
    }

    /// <summary>Delete a user master map entry.</summary>
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct = default)
    {
        await _mediator.Send(new DeleteUserMapCommand(id), ct);
        return NoContent();
    }
}
