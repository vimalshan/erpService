using CourseService.Application.DTOs;
using MediatR;

namespace CourseService.Application.Courses.Queries.GetCourse;

public record GetCourseQuery(long CourseId) : IRequest<CourseDto?>;
