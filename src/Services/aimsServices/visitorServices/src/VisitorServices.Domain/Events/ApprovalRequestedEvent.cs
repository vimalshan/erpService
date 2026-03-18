using VisitorServices.Domain.Common;

namespace VisitorServices.Domain.Events;

public sealed record ApprovalRequestedEvent : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public long ApprovalRequestId { get; init; }
    public long VisitorId { get; init; }
    public long RequiredApproverId { get; init; }
    public long RequestedBy { get; init; }

    // Parameterless constructor required for MassTransit deserialization
    public ApprovalRequestedEvent() { }

    public ApprovalRequestedEvent(long approvalRequestId, long visitorId, long requiredApproverId, long requestedBy)
    {
        ApprovalRequestId = approvalRequestId;
        VisitorId = visitorId;
        RequiredApproverId = requiredApproverId;
        RequestedBy = requestedBy;
    }
}
