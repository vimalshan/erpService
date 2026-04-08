using SciTransactional.Domain.Common;

namespace SciTransactional.Domain.Events;

public sealed record NavigationCreatedEvent(
    long RequestNum, string UserId, string SciId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record NavigationStatusChangedEvent(
    long RequestNum, string NewStatus) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record NormCreatedEvent(
    long NormNo, DateTime EffectiveDate) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record NormClosedEvent(
    long NormNo, DateTime ClosureDate) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record LicenseCreatedEvent(
    long LicenseId, string? LicenseNo) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record LicenseUpdatedEvent(
    long LicenseId, string? LicenseNo) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
