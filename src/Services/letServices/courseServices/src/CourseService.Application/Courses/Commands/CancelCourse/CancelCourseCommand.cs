using MediatR;

namespace CourseService.Application.Courses.Commands.CancelCourse;

public record CancelCourseCommand(long CourseId, DateTime CancellationDate, string CancellationRemark) : IRequest<bool>;
