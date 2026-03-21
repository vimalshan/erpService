using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdminService.Application.Commands.AccessRights;
using AdminService.Application.DTOs;
using AdminService.Application.Queries;

namespace AdminService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminAccessRightsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminAccessRightsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AdminAccessRightsDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllAccessRightsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AdminAccessRightsDto>> GetById(string id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAccessRightsByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-location/{locationId}")]
    public async Task<ActionResult<IReadOnlyList<AdminAccessRightsDto>>> GetByLocation(string locationId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAccessRightsByLocationQuery(locationId), ct);
        return Ok(result);
    }

    [HttpGet("{rightsId}/logs")]
    public async Task<ActionResult<IReadOnlyList<AdminAccessRightsLogDto>>> GetLogs(string rightsId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAccessRightsLogsByRightsIdQuery(rightsId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AdminAccessRightsDto>> Create([FromBody] CreateAccessRightsCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.AdminRightsId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AdminAccessRightsDto>> Update(string id, [FromBody] UpdateAccessRightsCommand command, CancellationToken ct)
    {
        if (id != command.AdminRightsId) return BadRequest("ID mismatch.");
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteAccessRightsCommand(id), ct);
        return NoContent();
    }
}
