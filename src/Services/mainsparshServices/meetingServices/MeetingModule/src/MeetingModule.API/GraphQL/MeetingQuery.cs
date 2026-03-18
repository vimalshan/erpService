using MediatR;
using MeetingModule.Application.DTOs;
using MeetingModule.Application.Queries.Meetings;
using MeetingModule.Application.Queries.MeetingTypes;
using MeetingModule.Application.Queries.Polls;

namespace MeetingModule.API.GraphQL;

public class MeetingQuery
{
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<MeetingTypeDto>> GetMeetingTypes([Service] IMediator mediator) =>
        await mediator.Send(new GetAllMeetingTypesQuery());

    public async Task<MeetingTypeDto?> GetMeetingTypeById([Service] IMediator mediator, long id) =>
        await mediator.Send(new GetMeetingTypeByIdQuery(id));

    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<MeetingScheduleDto>> GetMeetings([Service] IMediator mediator) =>
        await mediator.Send(new GetAllMeetingsQuery());

    public async Task<MeetingScheduleDto?> GetMeetingById([Service] IMediator mediator, long id) =>
        await mediator.Send(new GetMeetingByIdQuery(id));

    public async Task<IReadOnlyList<MeetingScheduleDto>> GetMeetingsByStatus([Service] IMediator mediator, string status) =>
        await mediator.Send(new GetMeetingsByStatusQuery(status));

    public async Task<IReadOnlyList<PollDetailDto>> GetPollsByMeeting([Service] IMediator mediator, long meetingId) =>
        await mediator.Send(new GetPollsByMeetingIdQuery(meetingId));
}
