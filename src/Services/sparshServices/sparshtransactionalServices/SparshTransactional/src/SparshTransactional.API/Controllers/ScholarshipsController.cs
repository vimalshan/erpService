using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparshTransactional.Application.Commands;
using SparshTransactional.Application.DTOs;
using SparshTransactional.Application.Queries;

namespace SparshTransactional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScholarshipsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ScholarshipMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllScholarshipsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(IReadOnlyList<ScholarshipMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken ct)
    {
        var result = await mediator.Send(new GetActiveScholarshipsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ScholarshipMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetScholarshipByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ScholarshipMasterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateScholarshipCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ScholarshipId }, result);
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ScholarshipMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateScholarshipCommand command, CancellationToken ct)
    {
        if (id != command.ScholarshipId) return BadRequest("ID mismatch.");
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPost("{id:long}/deactivate")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Deactivate(long id, [FromBody] long updatedBy, CancellationToken ct)
    {
        await mediator.Send(new DeactivateScholarshipCommand(id, updatedBy), ct);
        return NoContent();
    }

    [HttpGet("{id:long}/criteria")]
    [ProducesResponseType(typeof(IReadOnlyList<EligibilityCriteriaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCriteria(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetEligibilityCriteriaByScholarshipQuery(id), ct);
        return Ok(result);
    }

    [HttpPost("{id:long}/criteria")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(EligibilityCriteriaDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddCriteria(long id, [FromBody] AddEligibilityCriteriaCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command with { ScholarshipId = id }, ct);
        return CreatedAtAction(nameof(GetCriteria), new { id }, result);
    }
}
