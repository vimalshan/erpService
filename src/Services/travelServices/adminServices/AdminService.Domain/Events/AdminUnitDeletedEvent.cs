namespace AdminService.Domain.Events;

/// <summary>
/// Event raised when an admin unit is deleted
/// </summary>
public record AdminUnitDeletedEvent(
    long AdminCode,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);
