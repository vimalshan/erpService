using MediatR;
using MeetingModule.Application.DTOs;

namespace MeetingModule.Application.Queries.Meetings;

public record GetAllMeetingsQuery : IRequest<IReadOnlyList<MeetingScheduleDto>>;
public record GetMeetingByIdQuery(long Id) : IRequest<MeetingScheduleDto?>;
public record GetMeetingsByDateRangeQuery(DateTime From, DateTime To) : IRequest<IReadOnlyList<MeetingScheduleDto>>;
public record GetMeetingsByStatusQuery(string Status) : IRequest<IReadOnlyList<MeetingScheduleDto>>;
public record GetMeetingsByOrganizerQuery(long OrganizerId) : IRequest<IReadOnlyList<MeetingScheduleDto>>;
