using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RequestServices.Application.Commands.ApproveRequest;
using RequestServices.Application.Commands.CancelRequest;
using RequestServices.Application.Commands.CreateRequest;
using RequestServices.Application.DTOs;
using RequestServices.Application.Queries.GetPendingRequests;
using RequestServices.Application.Queries.GetRequestById;

namespace RequestServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class RequestsController(IMediator mediator) : ControllerBase
{
    /// <summary>Get a training request by ID.</summary>
    [HttpGet("{requestId:long}")]
    [ProducesResponseType(typeof(RequestMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long requestId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetRequestByIdQuery(requestId), ct);
        return Ok(result);
    }

    /// <summary>Get pending requests for a supervisor.</summary>
    [HttpGet("pending/{supervisorUser}")]
    [ProducesResponseType(typeof(IEnumerable<PendingRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending(string supervisorUser, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPendingRequestsQuery(supervisorUser), ct);
        return Ok(result);
    }

    /// <summary>Create a new training request.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(RequestMainDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRequestDto dto, CancellationToken ct)
    {
        var command = new CreateRequestCommand(
            dto.RequestId, dto.EmployeeUser, dto.RequestDate, dto.SupervisorUser,
            dto.TrainingNeed, dto.CourseId, dto.CourseDescription,
            dto.StartDate, dto.EndDate, dto.BusinessBenefit, dto.ExpectedCompetency);

        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { requestId = result.RequestId }, result);
    }

    /// <summary>Approve a training request line item.</summary>
    [HttpPost("{requestId:long}/approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(long requestId, [FromBody] ApproveRequestDto dto, CancellationToken ct)
    {
        var command = new ApproveRequestCommand(
            requestId, dto.SerialNumber, dto.ApprovalNumber, dto.ApprovalRemark, dto.ApprovalUser);

        await mediator.Send(command, ct);
        return Ok(new { message = "Request approved successfully." });
    }

    /// <summary>Cancel a training request line item.</summary>
    [HttpPost("{requestId:long}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(long requestId, [FromBody] CancelRequestDto dto, CancellationToken ct)
    {
        await mediator.Send(new CancelRequestCommand(requestId, dto.SerialNumber, dto.Remark), ct);
        return Ok(new { message = "Request cancelled successfully." });
    }
}
