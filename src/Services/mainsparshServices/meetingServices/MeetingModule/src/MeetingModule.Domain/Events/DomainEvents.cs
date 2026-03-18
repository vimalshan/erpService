using MediatR;
using MeetingModule.Domain.Entities;

namespace MeetingModule.Domain.Events;

public sealed record MeetingCreatedEvent(MeetingSchedule Meeting) : INotification;

public sealed record MeetingStatusChangedEvent(MeetingSchedule Meeting, string NewStatus) : INotification;

public sealed record MeetingTypeCreatedEvent(MeetingType MeetingType) : INotification;

public sealed record PollCreatedEvent(PollDetail Poll) : INotification;

public sealed record PollClosedEvent(PollDetail Poll) : INotification;
