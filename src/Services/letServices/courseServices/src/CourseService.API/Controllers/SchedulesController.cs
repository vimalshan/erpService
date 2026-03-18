using CourseService.Application.Schedules.Commands.CreateSchedule;
using CourseService.Application.Schedules.Queries.GetSchedules;
using CourseService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Controllers;

[ApiController]
[Route("api/courses/{courseId:long}/schedules")]
[Authorize]
[Produces("application/json")]
public class SchedulesController(IMediator mediator) : ControllerBase
{
    /// <summary>Gets all schedules for a course.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourseScheduleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(long courseId, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetSchedulesQuery(courseId), ct);
        return Ok(result);
    }

    /// <summary>Creates a new schedule session for a course.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(CourseScheduleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(long courseId, [FromBody] CreateScheduleRequest request, CancellationToken ct = default)
    {
        var command = new CreateScheduleCommand(
            courseId, request.ScheduleSerialNumber, request.ScheduleDate,
            request.StartTime, request.EndTime, request.LocationName, request.TrainerName);
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), new { courseId }, result);
    }
}

public record CreateScheduleRequest(
    long ScheduleSerialNumber,
    DateTime ScheduleDate,
    string StartTime,
    string EndTime,
    string LocationName,
    string TrainerName);
