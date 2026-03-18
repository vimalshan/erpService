using StipendService.Domain.Common;
using StipendService.Domain.Entities;

namespace StipendService.Domain.Events;

public sealed record StipendMasterCreatedEvent(StipendMaster StipendMaster) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record StipendMasterUpdatedEvent(StipendMaster StipendMaster) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record StipendMasterDeactivatedEvent(StipendMaster StipendMaster) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record DisbursementCreatedEvent(StipendDisbursement Disbursement) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record DisbursementProcessedEvent(StipendDisbursement Disbursement) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record DisbursementRejectedEvent(StipendDisbursement Disbursement) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
