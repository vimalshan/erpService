namespace EmailNotification.Domain.Events;

/// <summary>
/// Event raised when an email type is created
/// </summary>
public class EmailTypeCreatedEvent : Common.IDomainEvent
{
    /// <summary>
    /// Email type ID
    /// </summary>
    public long AggregateId { get; }

    /// <summary>
    /// Email name
    /// </summary>
    public string EmailName { get; }

    /// <summary>
    /// Email type (Daily or Event)
    /// </summary>
    public ValueObjects.EmailTypeEnum EmailType { get; }

    /// <summary>
    /// Procedure name
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
    /// Initializes a new instance of the EmailTypeCreatedEvent class
    /// </summary>
    public EmailTypeCreatedEvent(
        long aggregateId,
        string emailName,
        ValueObjects.EmailTypeEnum emailType,
        string procName)
    {
        AggregateId = aggregateId;
        EmailName = emailName;
        EmailType = emailType;
        ProcName = procName;
        OccurredAt = DateTime.UtcNow;
        EventId = Guid.NewGuid();
    }
}
