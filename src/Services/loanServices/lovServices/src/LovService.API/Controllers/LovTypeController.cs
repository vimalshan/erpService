using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LovService.Application.DTOs;
using LovService.Application.Features.LovTypeMast.Commands;
using LovService.Application.Features.LovTypeMast.Queries;

namespace LovService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[Produces("application/json")]
public class LovTypeController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<LovTypeMastDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int? orgId, CancellationToken ct)
        => Ok(await mediator.Send(new GetAllLovTypesQuery(orgId), ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType<LovTypeMastDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLovTypeByIdQuery(id), ct);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<LovTypeMastDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateLovTypeCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.LovTypeId }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType<LovTypeMastDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLovTypeCommand cmd, CancellationToken ct)
    {
        if (id != cmd.LovTypeId) return BadRequest();
        return Ok(await mediator.Send(cmd, ct));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await mediator.Send(new DeleteLovTypeCommand(id), ct);
        return deleted ? NoContent() : NotFound();
    }
}
