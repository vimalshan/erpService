using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdminService.Application.Commands.AdminMasters;
using AdminService.Application.DTOs;
using AdminService.Application.Queries;

namespace AdminService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminMasterController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminMasterController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AdminMasterDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllAdminMastersQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AdminMasterDto>> GetById(string id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAdminMasterByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AdminMasterDto>> Create([FromBody] CreateAdminMasterCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.AdminId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AdminMasterDto>> Update(string id, [FromBody] UpdateAdminMasterCommand command, CancellationToken ct)
    {
        if (id != command.AdminId) return BadRequest("ID mismatch.");
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteAdminMasterCommand(id), ct);
        return NoContent();
    }
}
