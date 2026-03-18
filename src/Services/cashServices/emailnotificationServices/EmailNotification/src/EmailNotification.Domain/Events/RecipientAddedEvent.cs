namespace EmailNotification.Domain.Events;

/// <summary>
/// Event raised when a recipient is added to an email type
/// </summary>
public class RecipientAddedEvent : Common.IDomainEvent
{
    /// <summary>
    /// Email type ID
    /// </summary>
    public long AggregateId { get; }

    /// <summary>
    /// Recipient email address
    /// </summary>
    public string EmailAddress { get; }

    /// <summary>
    /// Organization ID (if applicable)
    /// </summary>
    public long? OrgId { get; }

    /// <summary>
    /// Business unit ID (if applicable)
    /// </summary>
    public long? BusinessId { get; }

    /// <summary>
    /// When the event occurred
    /// </summary>
    public DateTime OccurredAt { get; }

    /// <summary>
    /// Unique event ID
    /// </summary>
    public Guid EventId { get; }

    /// <summary>
    /// Initializes a new instance of the RecipientAddedEvent class
    /// </summary>
    public RecipientAddedEvent(
        long aggregateId,
        string emailAddress,
        long? orgId = null,
        long? businessId = null)
    {
        AggregateId = aggregateId;
        EmailAddress = emailAddress;
        OrgId = orgId;
        BusinessId = businessId;
        OccurredAt = DateTime.UtcNow;
        EventId = Guid.NewGuid();
    }
}
