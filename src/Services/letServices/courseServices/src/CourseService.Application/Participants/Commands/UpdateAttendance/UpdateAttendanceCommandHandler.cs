using CourseService.Domain.Exceptions;
using CourseService.Domain.Interfaces;
using MediatR;

namespace CourseService.Application.Participants.Commands.UpdateAttendance;

public class UpdateAttendanceCommandHandler(ICourseRepository courseRepository, ICourseParticipantRepository participantRepository)
    : IRequestHandler<UpdateAttendanceCommand, bool>
{
    public async Task<bool> Handle(UpdateAttendanceCommand cmd, CancellationToken ct)
    {
        var course = await courseRepository.GetByIdAsync(cmd.CourseId, ct)
            ?? throw new CourseDomainException($"Course {cmd.CourseId} not found.");

        course.UpdateAttendance(cmd.UserCode, cmd.AttendanceStatus);

        var participant = await participantRepository.GetByUserCodeAsync(cmd.CourseId, cmd.UserCode, ct);
        if (participant is not null)
            await participantRepository.UpdateAsync(participant, ct);

        await courseRepository.UpdateAsync(course, ct);
        return true;
    }
}
