namespace EmailNotification.Domain.Events;

/// <summary>
/// Event raised when an email type is updated
/// </summary>
public class EmailTypeUpdatedEvent : Common.IDomainEvent
{
    /// <summary>
    /// Email type ID
    /// </summary>
    public long AggregateId { get; }

    /// <summary>
    /// New email name
    /// </summary>
    public string EmailName { get; }

    /// <summary>
    /// New procedure name
    /// </summary>
    public string ProcName { get; }

    /// <summary>
    /// When the event occurred
    /// </summary>
    public DateTime OccurredAt { get; }

    /// <summary>
    /// Unique event ID
    /// </summary>
    public Guid EventId { get; }

    /// <summary>
    /// Initializes a new instance of the EmailTypeUpdatedEvent class
    /// </summary>
    public EmailTypeUpdatedEvent(
        long aggregateId,
        string emailName,
        string procName)
    {
        AggregateId = aggregateId;
        EmailName = emailName;
        ProcName = procName;
        OccurredAt = DateTime.UtcNow;
        EventId = Guid.NewGuid();
    }
}
