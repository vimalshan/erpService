namespace AdminService.Domain.Events;

/// <summary>
/// Event raised when an admin unit is created
/// </summary>
public record AdminUnitCreatedEvent(
    long AdminCode,
    string AdminName,
    string? AdminType,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);
