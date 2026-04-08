using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SciTransactional.Application.Commands.CreateAdvanceLicense;
using SciTransactional.Application.Commands.UpdateAdvanceLicense;
using SciTransactional.Application.DTOs;
using SciTransactional.Application.Queries.GetAdvanceLicenseById;
using SciTransactional.Application.Queries.GetAllAdvanceLicenses;

namespace SciTransactional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class LicensesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AdvanceLicenseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllAdvanceLicensesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(AdvanceLicenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAdvanceLicenseByIdQuery(id), ct);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAdvanceLicenseCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateAdvanceLicenseCommand command, CancellationToken ct)
    {
        if (id != command.LicenseId)
            return BadRequest("ID mismatch.");
        await mediator.Send(command, ct);
        return NoContent();
    }
}
