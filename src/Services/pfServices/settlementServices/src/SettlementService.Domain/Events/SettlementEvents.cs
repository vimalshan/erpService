using SettlementService.Domain.Common;

namespace SettlementService.Domain.Events;

public record SettlementCreatedEvent(long SettlementNumber, long MemberNo, decimal Amount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record SettlementApprovedEvent(long SettlementNumber, long ApprovedBy) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record SettlementRejectedEvent(long SettlementNumber, long RejectedBy, string? Remarks) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record SettlementCompletedEvent(long SettlementNumber) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record SettlementPaymentAddedEvent(long SettlementNumber, decimal Amount, string PayMode) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
