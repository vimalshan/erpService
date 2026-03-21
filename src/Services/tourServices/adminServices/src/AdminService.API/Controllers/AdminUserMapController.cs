using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdminService.Application.Commands.UserMaps;
using AdminService.Application.DTOs;
using AdminService.Application.Queries;

namespace AdminService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminUserMapController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminUserMapController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AdminUserMapDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllAdminUserMapsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AdminUserMapDto>> GetById(string id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAdminUserMapByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-admin/{adminId}")]
    public async Task<ActionResult<IReadOnlyList<AdminUserMapDto>>> GetByAdminId(string adminId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAdminUserMapsByAdminIdQuery(adminId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AdminUserMapDto>> Create([FromBody] CreateAdminUserMapCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.AdminMapId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AdminUserMapDto>> Update(string id, [FromBody] UpdateAdminUserMapCommand command, CancellationToken ct)
    {
        if (id != command.AdminMapId) return BadRequest("ID mismatch.");
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteAdminUserMapCommand(id), ct);
        return NoContent();
    }
}
