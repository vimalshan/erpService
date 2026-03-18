using CourseService.Domain.Exceptions;
using CourseService.Domain.Interfaces;
using MediatR;

namespace CourseService.Application.Courses.Commands.CancelCourse;

public class CancelCourseCommandHandler(ICourseRepository repository) : IRequestHandler<CancelCourseCommand, bool>
{
    public async Task<bool> Handle(CancelCourseCommand cmd, CancellationToken ct)
    {
        var course = await repository.GetByIdAsync(cmd.CourseId, ct)
            ?? throw new CourseDomainException($"Course {cmd.CourseId} not found.");

        course.Cancel(cmd.CancellationDate, cmd.CancellationRemark);
        await repository.UpdateAsync(course, ct);
        return true;
    }
}
