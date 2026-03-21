using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourPlanService.Application.Commands.CreateForexRequisition;
using TourPlanService.Infrastructure.DapperQueries;

namespace TourPlanService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ForexController(
    IMediator mediator,
    TourPlanDapperRepository dapperRepo) : ControllerBase
{
    /// <summary>Get forex requisitions for a tour plan</summary>
    [HttpGet("tourplan/{tpId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTourPlan(string tpId, CancellationToken cancellationToken)
    {
        var result = await dapperRepo.GetForexByTourPlanIdAsync(tpId, cancellationToken);
        return Ok(result);
    }

    /// <summary>Create forex requisition</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateForexRequisitionCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetByTourPlan),
                new { tpId = command.TpId }, result.Value)
            : BadRequest(result.Error);
    }
}
