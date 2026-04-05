using MediatR;
using TransactionService.Domain.Common;

namespace TransactionService.Domain.Events;

public sealed record WorkflowSubmittedEvent(
    long WorkflowId,
    string EntityType,
    long EntityId,
    long EmployeeId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record WorkflowApprovedEvent(
    long WorkflowId,
    string EntityType,
    long EntityId,
    int ApprovalLevel,
    long ApproverId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record WorkflowRejectedEvent(
    long WorkflowId,
    string EntityType,
    long EntityId,
    int ApprovalLevel,
    long ApproverId,
    string? Remarks) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record WorkflowCancelledEvent(
    long WorkflowId,
    string EntityType,
    long EntityId,
    long CancelledBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record StepApprovedEvent(
    long WorkflowId,
    int StepLevel,
    long ApproverId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record StepRejectedEvent(
    long WorkflowId,
    int StepLevel,
    long ApproverId,
    string? Remarks) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record TransactionLoggedEvent(
    long LogId,
    string TransactionType,
    long TransactionId,
    string Action) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
