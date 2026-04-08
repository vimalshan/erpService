using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SciTransactional.Application.Commands.CloseNorm;
using SciTransactional.Application.Commands.CreateNorm;
using SciTransactional.Application.DTOs;
using SciTransactional.Application.Queries.GetAllNorms;
using SciTransactional.Application.Queries.GetNormById;

namespace SciTransactional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class NormsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<NormsMainDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllNormsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{normNo:long}")]
    [ProducesResponseType(typeof(NormsMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long normNo, CancellationToken ct)
    {
        var result = await mediator.Send(new GetNormByIdQuery(normNo), ct);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateNormCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { normNo = id }, id);
    }

    [HttpPost("{normNo:long}/close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Close(long normNo, CancellationToken ct)
    {
        await mediator.Send(new CloseNormCommand(normNo), ct);
        return NoContent();
    }
}
