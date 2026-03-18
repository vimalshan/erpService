namespace LoanApplication.Domain.Common;

/// <summary>
/// Base entity class for all domain entities
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Entity ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Created timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Modified timestamp
    /// </summary>
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Created by user ID
    /// </summary>
    public long CreatedBy { get; set; }

    /// <summary>
    /// Modified by user ID
    /// </summary>
    public long ModifiedBy { get; set; }

    /// <summary>
    /// Soft delete flag
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Domain events
    /// </summary>
    private readonly List<DomainEvent> _domainEvents = new();

    /// <summary>
    /// Get and clear domain events
    /// </summary>
    public IReadOnlyList<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    /// <summary>
    /// Clear domain events
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// Add domain event
    /// </summary>
    protected void RaiseDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
