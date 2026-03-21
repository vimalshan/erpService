using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourPlanService.Application.Commands.ApproveTourPlan;
using TourPlanService.Application.Commands.CreateTourPlan;
using TourPlanService.Application.Queries.GetTourPlanById;
using TourPlanService.Application.Queries.GetTourPlanList;

namespace TourPlanService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class TourPlanController(IMediator mediator) : ControllerBase
{
    /// <summary>Get all tour plans (paginated)</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? employeeId = null,
        [FromQuery] string? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new GetTourPlanListQuery(page, pageSize, employeeId, status), cancellationToken);
        return Ok(result);
    }

    /// <summary>Get tour plan by ID</summary>
    [HttpGet("{tpId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string tpId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetTourPlanByIdQuery(tpId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new tour plan</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTourPlanCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { tpId = result.Value }, result.Value)
            : BadRequest(result.Error);
    }

    /// <summary>Approve a tour plan</summary>
    [HttpPost("{tpId}/approve")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Approve(
        string tpId,
        [FromBody] ApproveRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new ApproveTourPlanCommand(tpId, request.ApprovedBy, request.Remarks), cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    /// <summary>Reject a tour plan</summary>
    [HttpPost("{tpId}/reject")]
    [Authorize(Roles = "Manager,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Reject(
        string tpId,
        [FromBody] RejectRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new RejectTourPlanCommand(tpId, request.RejectedBy, request.Remarks), cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}

public sealed record ApproveRequest(string ApprovedBy, string? Remarks = null);
public sealed record RejectRequest(string RejectedBy, string Remarks);
