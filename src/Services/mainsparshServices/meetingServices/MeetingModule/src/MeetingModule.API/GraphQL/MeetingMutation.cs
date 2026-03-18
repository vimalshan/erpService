using MediatR;
using MeetingModule.Application.Commands.Meetings;
using MeetingModule.Application.Commands.MeetingTypes;
using MeetingModule.Application.Commands.Polls;
using MeetingModule.Application.DTOs;

namespace MeetingModule.API.GraphQL;

public class MeetingMutation
{
    public async Task<MeetingTypeDto> CreateMeetingType(
        [Service] IMediator mediator, CreateMeetingTypeDto input, long userId) =>
        await mediator.Send(new CreateMeetingTypeCommand(input, userId));

    public async Task<MeetingTypeDto> UpdateMeetingType(
        [Service] IMediator mediator, long id, UpdateMeetingTypeDto input, long userId) =>
        await mediator.Send(new UpdateMeetingTypeCommand(id, input, userId));

    public async Task<MeetingScheduleDto> CreateMeeting(
        [Service] IMediator mediator, CreateMeetingScheduleDto input, long userId) =>
        await mediator.Send(new CreateMeetingCommand(input, userId));

    public async Task<MeetingScheduleDto> UpdateMeeting(
        [Service] IMediator mediator, long id, UpdateMeetingScheduleDto input, long userId) =>
        await mediator.Send(new UpdateMeetingCommand(id, input, userId));

    public async Task<PollDetailDto> CreatePoll(
        [Service] IMediator mediator, CreatePollDetailDto input, long userId) =>
        await mediator.Send(new CreatePollCommand(input, userId));
}
