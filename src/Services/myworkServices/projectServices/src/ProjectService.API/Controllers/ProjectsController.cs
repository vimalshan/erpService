using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectService.Application.Commands;
using ProjectService.Application.DTOs;
using ProjectService.Application.Queries;

namespace ProjectService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProjectMainDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllProjectsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ProjectMainDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProjectByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:long}/details")]
    public async Task<ActionResult<ProjectMainDto>> GetWithDetails(long id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProjectWithDetailsQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IReadOnlyList<ProjectMainDto>>> GetByStatus(char status, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProjectsByStatusQuery(status), cancellationToken);
        return Ok(result);
    }

    [HttpGet("leader/{leaderId:long}")]
    public async Task<ActionResult<IReadOnlyList<ProjectMainDto>>> GetByLeader(long leaderId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProjectsByLeaderQuery(leaderId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectMainDto>> Create([FromBody] CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.ProjId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ProjectMainDto>> Update(long id, [FromBody] UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        if (id != command.ProjId) return BadRequest("ID mismatch.");
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteProjectCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:long}/status")]
    public async Task<ActionResult<ProjectMainDto>> ChangeStatus(long id, [FromBody] char newStatus, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ChangeProjectStatusCommand(id, newStatus), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:long}/hold")]
    public async Task<ActionResult<ProjectHoldDto>> Hold(long id, [FromBody] HoldProjectCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command with { ProjId = id }, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:long}/close")]
    public async Task<ActionResult<ProjectMainDto>> Close(long id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CloseProjectCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}/members")]
    public async Task<ActionResult<IReadOnlyList<ProjectMemberDto>>> GetMembers(long id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProjectMembersQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:long}/members")]
    public async Task<ActionResult<ProjectMemberDto>> AddMember(long id, [FromBody] AddProjectMemberCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command with { ProjId = id }, cancellationToken);
        return CreatedAtAction(nameof(GetMembers), new { id }, result);
    }

    [HttpDelete("members/{memberId:long}")]
    public async Task<IActionResult> RemoveMember(long memberId, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveProjectMemberCommand(memberId), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:long}/status-history")]
    public async Task<ActionResult<ProjectStatusHistoryDto>> AddStatusHistory(long id, [FromBody] AddProjectStatusCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command with { ProjId = id }, cancellationToken);
        return Ok(result);
    }
}
