using CourseService.Application.DTOs;
using MediatR;

namespace CourseService.Application.Participants.Commands.RegisterParticipant;

public record RegisterParticipantCommand(
    long CourseId,
    string UserCode,
    long? NominationStatus,
    DateTime EnrollmentDate,
    char? ApprovalStatus
) : IRequest<CourseParticipantDto>;
