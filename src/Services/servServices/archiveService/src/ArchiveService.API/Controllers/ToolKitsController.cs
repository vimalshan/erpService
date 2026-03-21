using ArchiveService.Application.DTOs;
using ArchiveService.Application.Features.ToolKits.Commands;
using ArchiveService.Application.Features.ToolKits.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArchiveService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ToolKitsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ToolKitDto>> GetById(long id)
    {
        var result = await mediator.Send(new GetToolKitByIdQuery(id));
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ToolKitDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await mediator.Send(new GetToolKitsPagedQuery(page, pageSize));
        return Ok(result);
    }

    [HttpGet("engineer/{engineerId}")]
    public async Task<ActionResult<IReadOnlyList<ToolKitDto>>> GetByEngineer(string engineerId)
    {
        var result = await mediator.Send(new GetToolKitsByEngineerQuery(engineerId));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<long>> Create([FromBody] CreateToolKitCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:long}/flag")]
    public async Task<IActionResult> UpdateFlag(long id, [FromBody] UpdateToolKitFlagCommand command)
    {
        if (id != command.Id) return BadRequest("Id mismatch");
        var result = await mediator.Send(command);
        return result ? NoContent() : NotFound();
    }
}
