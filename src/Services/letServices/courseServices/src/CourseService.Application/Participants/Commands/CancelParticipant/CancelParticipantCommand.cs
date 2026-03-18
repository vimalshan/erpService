using MediatR;

namespace CourseService.Application.Participants.Commands.CancelParticipant;

public record CancelParticipantCommand(long CourseId, string UserCode, DateTime CancellationDate, string CancellationRemark) : IRequest<bool>;
