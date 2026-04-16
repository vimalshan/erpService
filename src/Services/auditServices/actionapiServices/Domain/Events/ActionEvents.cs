using MediatR;

namespace ActionService.Domain.Entities;

public class ActionCreatedEvent : IDomainEvent, INotification
{
    public ActionItem ActionItem { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ActionCreatedEvent(ActionItem actionItem)
    {
        ActionItem = actionItem;
    }
}

public class ActionCompletedEvent : IDomainEvent, INotification
{
    public ActionItem ActionItem { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ActionCompletedEvent(ActionItem actionItem)
    {
        ActionItem = actionItem;
    }
}
