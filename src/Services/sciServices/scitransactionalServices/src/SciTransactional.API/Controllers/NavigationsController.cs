using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SciTransactional.Application.Commands.CreateNavigation;
using SciTransactional.Application.Commands.UpdateNavigation;
using SciTransactional.Application.DTOs;
using SciTransactional.Application.Queries.GetAllNavigations;
using SciTransactional.Application.Queries.GetNavigationById;

namespace SciTransactional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class NavigationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<NavigationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllNavigationsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(NavigationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetNavigationByIdQuery(id), ct);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateNavigationCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:long}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateNavigationCommand command, CancellationToken ct)
    {
        if (id != command.RequestNum)
            return BadRequest("ID mismatch.");
        await mediator.Send(command, ct);
        return NoContent();
    }
}
