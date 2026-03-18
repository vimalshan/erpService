namespace LovService.Domain.Events;

public sealed record LovTypeCreatedEvent(long LovTypeId, string LovTypeName) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record LovMasterCreatedEvent(long LovId, long LovTypeId, string LovName) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record LovMasterUpdatedEvent(long LovId, string LovName, long UpdatedBy) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record LovTypeDeletedEvent(long LovTypeId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
