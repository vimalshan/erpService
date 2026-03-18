using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompetencyService.Application.Commands.Competencies;
using CompetencyService.Application.DTOs;
using CompetencyService.Application.Queries.Competencies;

namespace CompetencyService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompetenciesController(IMediator mediator) : ControllerBase
{
    /// <summary>Get all competencies (paged).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<CompetencyDto>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllCompetenciesQuery(page, pageSize), ct);
        return Ok(result);
    }

    /// <summary>Get a competency by ID.</summary>
    [HttpGet("{id:decimal}")]
    [ProducesResponseType(typeof(CompetencyDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(decimal id, CancellationToken ct)
    {
        var dto = await mediator.Send(new GetCompetencyByIdQuery(id), ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    /// <summary>Get competencies by type.</summary>
    [HttpGet("by-type/{type}")]
    [ProducesResponseType(typeof(IEnumerable<CompetencyDto>), 200)]
    public async Task<IActionResult> GetByType(string type, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCompetenciesByTypeQuery(type), ct);
        return Ok(result);
    }

    /// <summary>Create a new competency.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CompetencyDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateCompetencyCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update an existing competency.</summary>
    [HttpPut("{id:decimal}")]
    [ProducesResponseType(typeof(CompetencyDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(decimal id, [FromBody] UpdateCompetencyCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest("ID mismatch.");
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }

    /// <summary>Close a competency.</summary>
    [HttpPatch("{id:decimal}/close")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Close(decimal id, [FromBody] CloseCompetencyCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest("ID mismatch.");
        await mediator.Send(command, ct);
        return Ok();
    }

    /// <summary>Delete a competency.</summary>
    [HttpDelete("{id:decimal}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(decimal id, CancellationToken ct)
    {
        await mediator.Send(new DeleteCompetencyCommand(id), ct);
        return NoContent();
    }
}
