using CourseService.Application.Courses.Queries.GetCourse;
using CourseService.Application.Courses.Queries.GetCourses;
using CourseService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CourseService.API.MinimalApis;

/// <summary>
/// Minimal API endpoints for the Course module (alternative lightweight routes).
/// </summary>
public static class CourseMinimalApis
{
    public static WebApplication MapCourseMinimalApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/courses")
            .WithTags("Courses (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, int page = 1, int pageSize = 20, CancellationToken ct = default) =>
        {
            var result = await mediator.Send(new GetCoursesQuery(page, pageSize), ct);
            return Results.Ok(result);
        })
        .WithName("GetCoursesMinimal")
        .WithSummary("Get all courses (Minimal API)")
        .Produces<IEnumerable<CourseSummaryDto>>();

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCourseQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetCourseByIdMinimal")
        .WithSummary("Get course by ID (Minimal API)")
        .Produces<CourseDto>()
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
