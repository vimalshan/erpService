using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevelopmentService.Application.DTOs;
using DevelopmentService.Application.Queries.GetCompetencyIndicators;

namespace DevelopmentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompetencyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompetencyController(IMediator mediator) => _mediator = mediator;

    /// <summary>Gets competency indicators filtered by competency number and/or band.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CompetencyIndDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIndicators(
        [FromQuery] long? compNum, [FromQuery] string? band, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCompetencyIndicatorsQuery(compNum, band), ct);
        return Ok(result);
    }
}
