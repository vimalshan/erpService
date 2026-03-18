using MediatR;
using MeetingModule.Application.DTOs;

namespace MeetingModule.Application.Commands.Meetings;

public record CreateMeetingCommand(CreateMeetingScheduleDto Dto, long UserId) : IRequest<MeetingScheduleDto>;
public record UpdateMeetingCommand(long Id, UpdateMeetingScheduleDto Dto, long UserId) : IRequest<MeetingScheduleDto>;
public record StartMeetingCommand(long Id, long UserId) : IRequest<Unit>;
public record CompleteMeetingCommand(long Id, long UserId) : IRequest<Unit>;
public record CancelMeetingCommand(long Id, long UserId) : IRequest<Unit>;
