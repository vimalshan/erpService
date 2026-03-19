using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApprovalGroup.Application.ApprovalGroups.Commands;
using ApprovalGroup.Application.ApprovalGroups.Queries;
using ApprovalGroup.Application.DTOs;

namespace ApprovalGroup.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ApprovalGroupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprovalGroupsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all approval groups</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ApprovalGroupDto>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllApprovalGroupsQuery(), ct);
        return Ok(result);
    }

    /// <summary>Get approval group by ID</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApprovalGroupDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetApprovalGroupByIdQuery(id), ct);
        return Ok(result);
    }

    /// <summary>Create a new approval group</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApprovalGroupDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateApprovalGroupCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.GroupId }, result);
    }

    /// <summary>Update an approval group</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApprovalGroupDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateApprovalGroupCommand command, CancellationToken ct)
    {
        if (id != command.GroupId) return BadRequest("ID mismatch.");
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    /// <summary>Delete an approval group</summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteApprovalGroupCommand(id), ct);
        return NoContent();
    }
}
