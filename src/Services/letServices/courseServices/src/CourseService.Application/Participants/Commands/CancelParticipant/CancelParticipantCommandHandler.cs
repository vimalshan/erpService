using CourseService.Domain.Exceptions;
using CourseService.Domain.Interfaces;
using MediatR;

namespace CourseService.Application.Participants.Commands.CancelParticipant;

public class CancelParticipantCommandHandler(ICourseRepository courseRepository, ICourseParticipantRepository participantRepository)
    : IRequestHandler<CancelParticipantCommand, bool>
{
    public async Task<bool> Handle(CancelParticipantCommand cmd, CancellationToken ct)
    {
        var course = await courseRepository.GetByIdAsync(cmd.CourseId, ct)
            ?? throw new CourseDomainException($"Course {cmd.CourseId} not found.");

        course.CancelParticipant(cmd.UserCode, cmd.CancellationDate, cmd.CancellationRemark);

        var participant = await participantRepository.GetByUserCodeAsync(cmd.CourseId, cmd.UserCode, ct);
        if (participant is not null)
            await participantRepository.UpdateAsync(participant, ct);

        await courseRepository.UpdateAsync(course, ct);
        return true;
    }
}
