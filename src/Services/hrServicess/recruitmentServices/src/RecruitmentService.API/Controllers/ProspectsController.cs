using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentService.Application.Commands.Prospects;
using RecruitmentService.Application.DTOs;
using RecruitmentService.Application.Queries.Prospects;

namespace RecruitmentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProspectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProspectsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all prospects. Requires HR role.</summary>
    [HttpGet]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IEnumerable<ProspectSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllProspectsQuery(), ct));

    /// <summary>Get prospect by ID.</summary>
    [HttpGet("{id:decimal}")]
    [Authorize]
    [ProducesResponseType(typeof(ProspectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(decimal id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProspectByIdQuery(id), ct));

    /// <summary>Register a new prospect (public endpoint).</summary>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterProspectRequest request, CancellationToken ct)
    {
        var id = await _mediator.Send(new RegisterProspectCommand(request), ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>Deactivate a prospect. Requires Admin role.</summary>
    [HttpDelete("{id:decimal}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(decimal id, CancellationToken ct)
    {
        await _mediator.Send(new DeactivateProspectCommand(id), ct);
        return NoContent();
    }
}
