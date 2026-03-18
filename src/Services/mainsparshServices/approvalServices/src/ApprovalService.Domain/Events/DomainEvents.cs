namespace ApprovalService.Domain.Events;

using ApprovalService.Domain.Common;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.ValueObjects;

/// <summary>
/// Domain event raised when an approval master is created
/// </summary>
public record ApprovalMasterCreatedEvent(
    long ApprovalMasterId,
    string Code,
    string Name,
    string Module,
    int Level) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

/// <summary>
/// Domain event raised when an approval master is updated
/// </summary>
public record ApprovalMasterUpdatedEvent(
    long ApprovalMasterId,
    string Code,
    string Name,
    int Level) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

/// <summary>
/// Domain event raised when an approval master status changes
/// </summary>
public record ApprovalMasterStatusChangedEvent(
    long ApprovalMasterId,
    string Code,
    ApprovalStatus Status) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

/// <summary>
/// Domain event raised when an approver is assigned
/// </summary>
public record ApproverAssignedEvent(
    long ApprovalMasterId,
    string ApprovalCode,
    long ApproverId,
    long EmployeeSysId,
    int ApproverLevel) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

/// <summary>
/// Domain event raised when an approver is removed
/// </summary>
public record ApproverRemovedEvent(
    long ApprovalMasterId,
    string ApprovalCode,
    long ApproverId,
    long EmployeeSysId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

/// <summary>
/// Domain event raised when an approver employee is created
/// </summary>
public record ApproverEmployeeCreatedEvent(
    long ApproverId,
    long ApprovalMasterId,
    long EmployeeSysId,
    int ApproverLevel,
    DateTime EffectiveFrom) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

/// <summary>
/// Domain event raised when an approver employee is updated
/// </summary>
public record ApproverEmployeeUpdatedEvent(
    long ApproverId,
    long ApprovalMasterId,
    long EmployeeSysId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

/// <summary>
/// Domain event raised when an approver employee status changes
/// </summary>
public record ApproverEmployeeStatusChangedEvent(
    long ApproverId,
    long ApprovalMasterId,
    long EmployeeSysId,
    ApproverStatus Status) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
