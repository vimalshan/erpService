using MediatR;
using MeetingModule.Application.DTOs;

namespace MeetingModule.Application.Commands.MeetingTypes;

public record CreateMeetingTypeCommand(CreateMeetingTypeDto Dto, long UserId) : IRequest<MeetingTypeDto>;
public record UpdateMeetingTypeCommand(long Id, UpdateMeetingTypeDto Dto, long UserId) : IRequest<MeetingTypeDto>;
public record ActivateMeetingTypeCommand(long Id, long UserId) : IRequest<Unit>;
public record DeactivateMeetingTypeCommand(long Id, long UserId) : IRequest<Unit>;
