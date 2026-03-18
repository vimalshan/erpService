using MediatR;
using MeetingModule.Application.DTOs;

namespace MeetingModule.Application.Commands.Polls;

public record CreatePollCommand(CreatePollDetailDto Dto, long UserId) : IRequest<PollDetailDto>;
public record UpdatePollCommand(long Id, UpdatePollDetailDto Dto, long UserId) : IRequest<PollDetailDto>;
public record ClosePollCommand(long Id, long UserId) : IRequest<Unit>;
