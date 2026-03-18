namespace Shared.Core.Domain;

/// <summary>
/// Base class for all domain events
/// Domain events represent something significant that happened in the business domain
/// </summary>
public abstract record DomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
    public string AggregateType { get; init; } = string.Empty;
    public object? AggregateId { get; init; }
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}
