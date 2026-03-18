using OrganizationSetup.Domain.Common;

namespace OrganizationSetup.Domain.Events;

public sealed record PpLimitSetEvent(long LimitId, long OrgId, string TranType, decimal? LimitAmt, int FinYear) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record PpLimitUpdatedEvent(long LimitId, decimal? NewLimitAmt, decimal? NewLimitAct) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record PpCertificateUploadedEvent(long LimitId, string BlobUrl) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
