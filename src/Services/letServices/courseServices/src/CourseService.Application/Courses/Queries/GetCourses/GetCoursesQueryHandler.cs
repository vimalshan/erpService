using CourseService.Application.DTOs;
using CourseService.Domain.Aggregates;
using CourseService.Domain.Interfaces;
using MediatR;

namespace CourseService.Application.Courses.Queries.GetCourses;

public class GetCoursesQueryHandler(ICourseRepository repository) : IRequestHandler<GetCoursesQuery, IEnumerable<CourseSummaryDto>>
{
    public async Task<IEnumerable<CourseSummaryDto>> Handle(GetCoursesQuery query, CancellationToken ct)
    {
        IEnumerable<CourseAggregate> courses;

        if (query.CourseType.HasValue)
            courses = await repository.GetByCourseTypeAsync(query.CourseType.Value, ct);
        else
            courses = await repository.GetAllAsync(query.Page, query.PageSize, ct);

        return courses.Select(c => new CourseSummaryDto(
            c.CourseId,
            c.CourseDescription,
            c.CourseType,
            c.Duration.StartDate,
            c.Duration.EndDate,
            c.Duration.NumberOfDays,
            c.Participants.Count,
            c.Schedules.Count,
            c.CancellationDate.HasValue));
    }
}
