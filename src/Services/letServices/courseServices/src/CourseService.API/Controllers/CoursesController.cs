using CourseService.Application.Courses.Commands.CancelCourse;
using CourseService.Application.Courses.Commands.CreateCourse;
using CourseService.Application.Courses.Commands.DeleteCourse;
using CourseService.Application.Courses.Queries.GetCourse;
using CourseService.Application.Courses.Queries.GetCourses;
using CourseService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CoursesController(IMediator mediator) : ControllerBase
{
    /// <summary>Gets all courses with optional filtering by type and pagination.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourseSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] char? courseType = null,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetCoursesQuery(page, pageSize, courseType), ct);
        return Ok(result);
    }

    /// <summary>Gets a specific course by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetCourseQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Creates a new course.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCourseCommand command, CancellationToken ct = default)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.CourseId }, result);
    }

    /// <summary>Cancels a course.</summary>
    [HttpPost("{id:long}/cancel")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(long id, [FromBody] CancelCourseRequest request, CancellationToken ct = default)
    {
        await mediator.Send(new CancelCourseCommand(id, request.CancellationDate, request.CancellationRemark), ct);
        return NoContent();
    }

    /// <summary>Deletes a course.</summary>
    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct = default)
    {
        await mediator.Send(new DeleteCourseCommand(id), ct);
        return NoContent();
    }
}

public record CancelCourseRequest(DateTime CancellationDate, string CancellationRemark);
