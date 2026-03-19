using HRDocumentService.Domain.Common;

namespace HRDocumentService.Domain.Events;

public sealed record DocumentCreatedEvent(long DocId, long DocNo, string DocType) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record DocumentSubmittedEvent(long DocId, long DocNo) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record DocumentApprovedEvent(long DocId, long DocNo, decimal ApprovedBy) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record DocumentRejectedEvent(long DocId, long DocNo, decimal RejectedBy, string RejectRemarks) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record DocumentCancelledEvent(long DocId, long DocNo, decimal CancelledBy) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
