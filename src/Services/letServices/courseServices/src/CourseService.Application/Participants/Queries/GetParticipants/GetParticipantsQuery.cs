using CourseService.Application.DTOs;
using MediatR;

namespace CourseService.Application.Participants.Queries.GetParticipants;

public record GetParticipantsQuery(long CourseId) : IRequest<IEnumerable<CourseParticipantDto>>;
