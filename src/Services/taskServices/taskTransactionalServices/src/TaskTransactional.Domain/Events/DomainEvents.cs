using TaskTransactional.Domain.Common;

namespace TaskTransactional.Domain.Events;

public record ComplaintCreatedEvent(string GroupId, string GroupName, string UnitCode) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record ComplaintUpdatedEvent(string GroupId, string GroupName) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record TicketCreatedEvent(decimal TicketNum, string Subject) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record TicketClosedEvent(decimal TicketNum) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record ActionCreatedEvent(decimal ActionNum, decimal TaskNum) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record ActionUpdatedEvent(decimal ActionNum, string ActionLevel) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
