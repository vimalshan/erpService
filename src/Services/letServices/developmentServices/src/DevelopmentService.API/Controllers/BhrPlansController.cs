using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevelopmentService.Application.Commands.CreateBhrPlan;
using DevelopmentService.Application.DTOs;

namespace DevelopmentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BhrPlansController : ControllerBase
{
    private readonly IMediator _mediator;

    public BhrPlansController(IMediator mediator) => _mediator = mediator;

    /// <summary>Creates a BHR-approved training plan.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(LetBhrPlanDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBhrPlanCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(Create), new { reqNum = result.ReqNum }, result);
    }
}
