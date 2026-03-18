using VisitorServices.Domain.Common;

namespace VisitorServices.Domain.Events;

public sealed record VisitorRegisteredEvent : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public long VisitorId { get; init; }
    public string VisitorName { get; init; } = string.Empty;
    public long RegisteredBy { get; init; }

    public VisitorRegisteredEvent() { }

    public VisitorRegisteredEvent(long visitorId, string visitorName, long registeredBy)
    {
        VisitorId = visitorId;
        VisitorName = visitorName;
        RegisteredBy = registeredBy;
    }
}
