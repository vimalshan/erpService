namespace AdminService.Domain.Events;

/// <summary>
/// Event raised when a finance unit is created
/// </summary>
public record FinanceUnitCreatedEvent(
    long UnitId,
    string UnitCode,
    string UnitName,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);
