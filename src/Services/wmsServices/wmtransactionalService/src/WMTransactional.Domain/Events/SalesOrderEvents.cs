using WMTransactional.Domain.Common;

namespace WMTransactional.Domain.Events;

public sealed record SalesOrderCreatedEvent(string SoNumber, int CustomerId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record SalesOrderConfirmedEvent(string SoNumber, int CustomerId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record SalesOrderCompletedEvent(string SoNumber, int CustomerId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record SalesOrderCancelledEvent(string SoNumber, int CustomerId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
