using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SciTransactional.Application.Commands.CreateDirectEntry;
using SciTransactional.Application.DTOs;
using SciTransactional.Application.Queries.GetDirectEntries;

namespace SciTransactional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class DirectEntriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DirectEntryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetDirectEntriesQuery(), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateDirectEntryCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return StatusCode(StatusCodes.Status201Created, id);
    }
}
