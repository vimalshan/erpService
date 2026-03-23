namespace AttendanceService.Domain.Common;

public abstract class BaseAggregateRoot : BaseEntity
{
    // Aggregate roots own the lifecycle of their entities.
    // Domain events are already managed via BaseEntity.
}
