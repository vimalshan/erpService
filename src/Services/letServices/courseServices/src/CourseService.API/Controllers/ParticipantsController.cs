using CourseService.Application.Participants.Commands.CancelParticipant;
using CourseService.Application.Participants.Commands.RegisterParticipant;
using CourseService.Application.Participants.Commands.UpdateAttendance;
using CourseService.Application.Participants.Queries.GetParticipants;
using CourseService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Controllers;

[ApiController]
[Route("api/courses/{courseId:long}/participants")]
[Authorize]
[Produces("application/json")]
public class ParticipantsController(IMediator mediator) : ControllerBase
{
    /// <summary>Gets all participants for a course.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourseParticipantDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(long courseId, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetParticipantsQuery(courseId), ct);
        return Ok(result);
    }

    /// <summary>Registers a participant for a course.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CourseParticipantDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(long courseId, [FromBody] RegisterParticipantRequest request, CancellationToken ct = default)
    {
        var command = new RegisterParticipantCommand(courseId, request.UserCode, request.NominationStatus, request.EnrollmentDate, request.ApprovalStatus);
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), new { courseId }, result);
    }

    /// <summary>Cancels a participant's registration.</summary>
    [HttpPost("{userCode}/cancel")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Cancel(long courseId, string userCode, [FromBody] CancelParticipantRequest request, CancellationToken ct = default)
    {
        await mediator.Send(new CancelParticipantCommand(courseId, userCode, request.CancellationDate, request.CancellationRemark), ct);
        return NoContent();
    }

    /// <summary>Updates attendance status for a participant.</summary>
    [HttpPut("{userCode}/attendance")]
    [Authorize(Roles = "Admin,Manager,Trainer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateAttendance(long courseId, string userCode, [FromBody] UpdateAttendanceRequest request, CancellationToken ct = default)
    {
        await mediator.Send(new UpdateAttendanceCommand(courseId, userCode, request.AttendanceStatus), ct);
        return NoContent();
    }
}

public record RegisterParticipantRequest(string UserCode, long? NominationStatus, DateTime EnrollmentDate, char? ApprovalStatus);
public record CancelParticipantRequest(DateTime CancellationDate, string CancellationRemark);
public record UpdateAttendanceRequest(char AttendanceStatus);
