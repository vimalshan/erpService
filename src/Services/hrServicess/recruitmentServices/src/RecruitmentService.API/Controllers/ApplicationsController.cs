using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using RecruitmentService.Application.Commands.Applications;
using RecruitmentService.Application.DTOs;
using RecruitmentService.Application.Queries.Applications;
using RecruitmentService.Infrastructure.Messaging.Consumers;

namespace RecruitmentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ApplicationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;

    public ApplicationsController(IMediator mediator, IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    /// <summary>Get application by ID.</summary>
    [HttpGet("{id:decimal}")]
    [Authorize]
    [ProducesResponseType(typeof(ApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(decimal id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApplicationByIdQuery(id), ct));

    /// <summary>Get all applications for a vacancy. Requires HR role.</summary>
    [HttpGet("vacancy/{vacancyId:decimal}")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IEnumerable<ApplicationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByVacancy(decimal vacancyId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetApplicationsByVacancyQuery(vacancyId), ct));

    /// <summary>Submit an application for a vacancy.</summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Submit([FromBody] SubmitApplicationRequest request, CancellationToken ct)
    {
        var submittedBy = GetCurrentUserId();
        var id = await _mediator.Send(new SubmitApplicationCommand(request, submittedBy), ct);

        await _publishEndpoint.Publish(
            new ApplicationSubmittedMessage(id, request.VacancyId, DateTime.UtcNow),
            ct);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>Update application status. Requires HR role.</summary>
    [HttpPatch("{id:decimal}/status")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(decimal id, [FromBody] UpdateStatusRequest request, CancellationToken ct)
    {
        await _mediator.Send(new UpdateApplicationStatusCommand(id, request.StatusCode, request.Remarks, GetCurrentUserId()), ct);

        await _publishEndpoint.Publish(
            new ApplicationStatusChangedMessage(id, "UNKNOWN", request.StatusCode, DateTime.UtcNow),
            ct);

        return NoContent();
    }

    private decimal GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return decimal.TryParse(claim, out var id) ? id : 0;
    }
}
