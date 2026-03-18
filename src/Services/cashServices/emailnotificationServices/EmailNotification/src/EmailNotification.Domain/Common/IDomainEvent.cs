using MediatR;

namespace EmailNotification.Domain.Common;

/// <summary>
/// Interface for domain events
/// </summary>
public interface IDomainEvent : INotification
{
    /// <summary>
    /// Gets the date and time when the event occurred
    /// </summary>
    DateTime OccurredAt { get; }

    /// <summary>
    /// Gets the event ID
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// Gets the aggregate root ID that raised this event
    /// </summary>
    long AggregateId { get; }
}
