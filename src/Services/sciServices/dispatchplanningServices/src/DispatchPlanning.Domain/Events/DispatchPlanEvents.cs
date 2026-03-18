using DispatchPlanning.Domain.Common;

namespace DispatchPlanning.Domain.Events;

public sealed record DispatchPlanCreatedEvent(
    int PlanHeaderId,
    char PlanType,
    DateTime PlanMonth,
    int CompanyUnitId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record DispatchPlanItemAddedEvent(
    int PlanHeaderId,
    int BreakupItemId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record DispatchPlanForecastUpdatedEvent(
    int PlanHeaderId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record DispatchPlanDeletedEvent(
    int PlanHeaderId,
    int DeletedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
