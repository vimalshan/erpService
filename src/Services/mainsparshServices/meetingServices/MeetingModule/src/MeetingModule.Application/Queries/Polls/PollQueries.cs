using MediatR;
using MeetingModule.Application.DTOs;

namespace MeetingModule.Application.Queries.Polls;

public record GetPollsByMeetingIdQuery(long MeetingId) : IRequest<IReadOnlyList<PollDetailDto>>;
public record GetPollByIdQuery(long Id) : IRequest<PollDetailDto?>;
