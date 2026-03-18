using CourseService.Domain.Exceptions;
using CourseService.Domain.Interfaces;
using MediatR;

namespace CourseService.Application.Courses.Commands.DeleteCourse;

public class DeleteCourseCommandHandler(ICourseRepository repository) : IRequestHandler<DeleteCourseCommand, bool>
{
    public async Task<bool> Handle(DeleteCourseCommand cmd, CancellationToken ct)
    {
        if (!await repository.ExistsAsync(cmd.CourseId, ct))
            throw new CourseDomainException($"Course {cmd.CourseId} not found.");

        await repository.DeleteAsync(cmd.CourseId, ct);
        return true;
    }
}
