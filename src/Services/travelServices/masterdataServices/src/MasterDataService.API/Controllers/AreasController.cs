using MasterDataService.Application.Commands.Area;
using MasterDataService.Application.DTOs;
using MasterDataService.Application.Queries.Area;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterDataService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AreasController : ControllerBase
{
    private readonly IMediator _mediator;

    public AreasController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<AreaDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllAreasQuery());
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public async Task<ActionResult<AreaDto>> GetById(long id)
    {
        var result = await _mediator.Send(new GetAreaByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AreaDto>> Create([FromBody] CreateAreaCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<AreaDto>> Update(long id, [FromBody] UpdateAreaCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteAreaCommand(id));
        return NoContent();
    }
}
