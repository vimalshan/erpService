using MediatR;

namespace CourseService.Application.Courses.Commands.DeleteCourse;

public record DeleteCourseCommand(long CourseId) : IRequest<bool>;
