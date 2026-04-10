using SSCTransactional.Domain.Common;

namespace SSCTransactional.Domain.Events;

public record AllocationCreatedDomainEvent(long AllocationId, long DocId, string Action, long GroupId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record AllocationPulledDomainEvent(long AllocationId, long PullUserId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record AllocationCompletedDomainEvent(long AllocationId, long DocId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record CorrespondenceCreatedDomainEvent(long CorrespondenceId, long DocId, string HoldCategory) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record CorrespondenceReleasedDomainEvent(long CorrespondenceId, long DocId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record ApprovalCreatedDomainEvent(long ApprovalId, long DocId, long ApproverId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record RescanRequestedDomainEvent(long RescanId, long DocId, long AllocationId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record DocumentRevokedDomainEvent(long RevokeId, long DocId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
