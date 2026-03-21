using ComplaintService.Domain.Common;

namespace ComplaintService.Domain.Events;

public sealed record ComplaintCreatedEvent(
    decimal TicketNum, decimal GroupId, decimal CreatedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ComplaintClosedEvent(
    decimal TicketNum, decimal ClosedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ComplaintReopenedEvent(
    decimal TicketNum, decimal ReopenedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ActionRecordedEvent(
    decimal TicketNum, char ActionLevel, decimal ActBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ComplaintEscalatedEvent(
    decimal TicketNum, decimal EscLevel, decimal EscalatedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
