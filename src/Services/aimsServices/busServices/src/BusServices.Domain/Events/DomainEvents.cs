using BusServices.Domain.Common;

namespace BusServices.Domain.Events;

public sealed record BusRegisteredEvent(
    int BusId,
    string RegistrationNumber,
    long RegisteredBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record BusArrivedEvent(
    long ArrivalId,
    int BusId,
    DateTime ArrivalDate,
    TimeOnly ArrivalTime,
    char Status) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record EmployeeAssignedToBusEvent(
    long EmpBusId,
    long EmpSysId,
    int BusId,
    int RouteId,
    long AssignedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record BusDeductionRateSetEvent(
    int DeductId,
    int BusId,
    decimal Amount,
    DateTime EffectiveDate) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
