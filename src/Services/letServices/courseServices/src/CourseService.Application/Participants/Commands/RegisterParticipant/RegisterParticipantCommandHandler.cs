using CourseService.Application.DTOs;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Interfaces;
using MediatR;

namespace CourseService.Application.Participants.Commands.RegisterParticipant;

public class RegisterParticipantCommandHandler(ICourseRepository courseRepository, ICourseParticipantRepository participantRepository)
    : IRequestHandler<RegisterParticipantCommand, CourseParticipantDto>
{
    public async Task<CourseParticipantDto> Handle(RegisterParticipantCommand cmd, CancellationToken ct)
    {
        var course = await courseRepository.GetByIdAsync(cmd.CourseId, ct)
            ?? throw new CourseDomainException($"Course {cmd.CourseId} not found.");

        var participant = course.RegisterParticipant(cmd.UserCode, cmd.NominationStatus, cmd.EnrollmentDate, cmd.ApprovalStatus);
        await participantRepository.AddAsync(participant, ct);
        await courseRepository.UpdateAsync(course, ct);

        return new CourseParticipantDto(
            participant.CourseId, participant.UserCode, participant.NominationStatus,
            participant.EnrollmentDate, participant.ApprovalStatus,
            participant.CancellationDate, participant.CancellationRemark,
            participant.AttendanceStatus, participant.UserPin,
            participant.ApproverCode, participant.ApproverPin);
    }
}
