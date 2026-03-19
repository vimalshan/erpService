using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApprovalGroup.Application.UserMaps.Commands;
using ApprovalGroup.Application.UserMaps.Queries;
using ApprovalGroup.Application.DTOs;

namespace ApprovalGroup.API.Controllers;

[ApiController]
[Route("api/approval-groups/{groupId:long}/users")]
[Authorize]
[Produces("application/json")]
public class UserMapsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserMapsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all user mappings for a group</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ApprovalGroupUserMapDto>), 200)]
    public async Task<IActionResult> GetByGroupId(long groupId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetUserMapsByGroupIdQuery(groupId), ct);
        return Ok(result);
    }

    /// <summary>Map a user to the approval group</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApprovalGroupUserMapDto), 201)]
    public async Task<IActionResult> MapUser(long groupId, [FromBody] MapUserRequest request, CancellationToken ct)
    {
        var command = new MapUserToGroupCommand(groupId, request.UserId, request.EffectiveDate, request.CreatedBy);
        var result = await _mediator.Send(command, ct);
        return Created(string.Empty, result);
    }

    /// <summary>Remove a user from the approval group</summary>
    [HttpDelete("{mapId:long}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> RemoveUser(long mapId, [FromQuery] long modifiedBy, CancellationToken ct)
    {
        await _mediator.Send(new RemoveUserFromGroupCommand(mapId, modifiedBy), ct);
        return NoContent();
    }
}

public record MapUserRequest(long UserId, DateTime EffectiveDate, long CreatedBy);
