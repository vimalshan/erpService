using CourseService.Application.Courses.Queries.GetCourse;
using CourseService.Application.Courses.Queries.GetCourses;
using CourseService.Application.DTOs;
using CourseService.Application.Participants.Queries.GetParticipants;
using CourseService.Application.Schedules.Queries.GetSchedules;
using MediatR;

namespace CourseService.API.GraphQL;

/// <summary>
/// GraphQL Query type - accessible at /graphql via Banana Cake Pop.
/// </summary>
public class CourseQuery
{
    public async Task<CourseDto?> GetCourse([Service] IMediator mediator, long courseId, CancellationToken ct)
        => await mediator.Send(new GetCourseQuery(courseId), ct);

    public async Task<IEnumerable<CourseSummaryDto>> GetCourses(
        [Service] IMediator mediator,
        int page = 1,
        int pageSize = 20,
        char? courseType = null,
        CancellationToken ct = default)
        => await mediator.Send(new GetCoursesQuery(page, pageSize, courseType), ct);

    public async Task<IEnumerable<CourseScheduleDto>> GetSchedules(
        [Service] IMediator mediator,
        long courseId,
        CancellationToken ct)
        => await mediator.Send(new GetSchedulesQuery(courseId), ct);

    public async Task<IEnumerable<CourseParticipantDto>> GetParticipants(
        [Service] IMediator mediator,
        long courseId,
        CancellationToken ct)
        => await mediator.Send(new GetParticipantsQuery(courseId), ct);
}
