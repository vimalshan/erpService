using MediatR;
using MeetingModule.Application.DTOs;

namespace MeetingModule.Application.Queries.MeetingTypes;

public record GetAllMeetingTypesQuery : IRequest<IReadOnlyList<MeetingTypeDto>>;
public record GetActiveMeetingTypesQuery : IRequest<IReadOnlyList<MeetingTypeDto>>;
public record GetMeetingTypeByIdQuery(long Id) : IRequest<MeetingTypeDto?>;
public record GetMeetingTypeByCodeQuery(string Code) : IRequest<MeetingTypeDto?>;
