using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelService.Application.TourPlans.Commands.ApproveTourPlan;
using TravelService.Application.TourPlans.Commands.CreateTourPlan;
using TravelService.Application.TourPlans.Commands.RejectTourPlan;
using TravelService.Application.TourPlans.Queries.GetTourPlan;
using TravelService.Application.TourPlans.Queries.GetTourPlanList;

namespace TravelService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class TourPlansController : ControllerBase
{
    private readonly ISender _sender;

    public TourPlansController(ISender sender) => _sender = sender;

    /// <summary>Get paginated list of tour plans.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? employeeSysId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetTourPlanListQuery(page, pageSize, employeeSysId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Get a tour plan by ID.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetTourPlanByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new tour plan.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTourPlanCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Approve a tour plan.</summary>
    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Admin,Manager,Approver")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(string id, [FromBody] ApproveRequest req, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ApproveTourPlanCommand(id, req.ApprovedBy, req.Remarks), cancellationToken);
        return Ok(result);
    }

    /// <summary>Reject a tour plan.</summary>
    [HttpPut("{id}/reject")]
    [Authorize(Roles = "Admin,Manager,Approver")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(string id, [FromBody] RejectRequest req, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new RejectTourPlanCommand(id, req.RejectedBy, req.Remarks), cancellationToken);
        return Ok(result);
    }
}

public record ApproveRequest(string ApprovedBy, string? Remarks);
public record RejectRequest(string RejectedBy, string Remarks);
