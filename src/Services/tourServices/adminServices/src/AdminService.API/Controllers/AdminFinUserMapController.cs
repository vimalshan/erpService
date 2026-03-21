using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdminService.Application.Commands.FinUserMaps;
using AdminService.Application.DTOs;
using AdminService.Application.Queries;

namespace AdminService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminFinUserMapController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminFinUserMapController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AdminFinUserMapDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllAdminFinUserMapsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AdminFinUserMapDto>> GetById(string id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAdminFinUserMapByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AdminFinUserMapDto>> Create([FromBody] CreateAdminFinUserMapCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.FinanceMapId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AdminFinUserMapDto>> Update(string id, [FromBody] UpdateAdminFinUserMapCommand command, CancellationToken ct)
    {
        if (id != command.FinanceMapId) return BadRequest("ID mismatch.");
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteAdminFinUserMapCommand(id), ct);
        return NoContent();
    }
}
