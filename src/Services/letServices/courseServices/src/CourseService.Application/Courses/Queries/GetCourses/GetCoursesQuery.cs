using CourseService.Application.DTOs;
using MediatR;

namespace CourseService.Application.Courses.Queries.GetCourses;

public record GetCoursesQuery(int Page = 1, int PageSize = 20, char? CourseType = null) : IRequest<IEnumerable<CourseSummaryDto>>;
