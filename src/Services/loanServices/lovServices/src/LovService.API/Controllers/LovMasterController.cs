using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LovService.Application.DTOs;
using LovService.Application.Features.LovMaster.Commands;
using LovService.Application.Features.LovMaster.Queries;

namespace LovService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[Produces("application/json")]
public class LovMasterController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<LovMasterDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int? lovTypeId, CancellationToken ct)
        => Ok(await mediator.Send(new GetAllLovMastersQuery(lovTypeId), ct));

    [HttpGet("{id:long}")]
    [ProducesResponseType<LovMasterDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLovMasterByIdQuery(id), ct);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<LovMasterDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateLovMasterCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.LovId }, result);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType<LovMasterDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateLovMasterCommand cmd, CancellationToken ct)
    {
        if (id != cmd.LovId) return BadRequest();
        return Ok(await mediator.Send(cmd, ct));
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var deleted = await mediator.Send(new DeleteLovMasterCommand(id), ct);
        return deleted ? NoContent() : NotFound();
    }
}
