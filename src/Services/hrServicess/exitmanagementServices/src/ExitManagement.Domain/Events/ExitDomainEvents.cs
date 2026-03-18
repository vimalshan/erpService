using ExitManagement.Domain.Common;

namespace ExitManagement.Domain.Events;

public sealed record ExitInitiatedEvent(
    decimal ExitNo,
    decimal EmployeeSysId,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}

public sealed record ExitApprovedEvent(
    decimal ExitNo,
    decimal EmployeeSysId,
    decimal ApprovedBy,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}

public sealed record ExitRevokedEvent(
    decimal ExitNo,
    decimal EmployeeSysId,
    string Reason,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}

public sealed record ExitInterviewCompletedEvent(
    decimal ExitNo,
    decimal EmployeeSysId,
    string ConductedBy,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}

public sealed record ExitFormalityCompletedEvent(
    decimal ExitNo,
    decimal EmployeeSysId,
    decimal CompletedBy,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
}
