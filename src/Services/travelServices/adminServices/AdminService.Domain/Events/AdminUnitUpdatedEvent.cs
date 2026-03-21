namespace AdminService.Domain.Events;

/// <summary>
/// Event raised when an admin unit is updated
/// </summary>
public record AdminUnitUpdatedEvent(
    long AdminCode,
    string AdminName,
    string? AdminType,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);
