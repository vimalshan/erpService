using ObjectiveService.Domain.Entities;
using MediatR;

namespace ObjectiveService.Domain.Events;

// ── Goal Domain Events ──────────────────────────────────────────────────────

public class GoalCreatedEvent : DomainEventBase, INotification
{
    public decimal GoalId { get; }
    public string UserId { get; }
    public DateTime PeriodFrom { get; }
    public DateTime PeriodTo { get; }

    public GoalCreatedEvent(decimal goalId, string userId, DateTime periodFrom, DateTime periodTo)
    {
        GoalId = goalId;
        UserId = userId;
        PeriodFrom = periodFrom;
        PeriodTo = periodTo;
    }
}

public class GoalSubmittedForApprovalEvent : DomainEventBase, INotification
{
    public decimal GoalId { get; }
    public string UserId { get; }

    public GoalSubmittedForApprovalEvent(decimal goalId, string userId)
    {
        GoalId = goalId;
        UserId = userId;
    }
}

public class GoalApprovedEvent : DomainEventBase, INotification
{
    public decimal GoalId { get; }
    public string UserId { get; }

    public GoalApprovedEvent(decimal goalId, string userId)
    {
        GoalId = goalId;
        UserId = userId;
    }
}

public class GoalReturnedEvent : DomainEventBase, INotification
{
    public decimal GoalId { get; }
    public string UserId { get; }
    public string Remarks { get; }

    public GoalReturnedEvent(decimal goalId, string userId, string remarks)
    {
        GoalId = goalId;
        UserId = userId;
        Remarks = remarks;
    }
}

public class GoalClosedEvent : DomainEventBase, INotification
{
    public decimal GoalId { get; }
    public string UserId { get; }

    public GoalClosedEvent(decimal goalId, string userId)
    {
        GoalId = goalId;
        UserId = userId;
    }
}

public class GoalAchievementRecordedEvent : DomainEventBase, INotification
{
    public decimal GoalId { get; }
    public decimal SubGoalId { get; }
    public string Achievement { get; }

    public GoalAchievementRecordedEvent(decimal goalId, decimal subGoalId, string achievement)
    {
        GoalId = goalId;
        SubGoalId = subGoalId;
        Achievement = achievement;
    }
}

// ── Control Point Domain Events ─────────────────────────────────────────────

public class ControlPointCreatedEvent : DomainEventBase, INotification
{
    public decimal ControlPointId { get; }
    public decimal EmployeeSysId { get; }
    public string Description { get; }

    public ControlPointCreatedEvent(decimal controlPointId, decimal employeeSysId, string description)
    {
        ControlPointId = controlPointId;
        EmployeeSysId = employeeSysId;
        Description = description;
    }
}

public class ControlPointModifiedEvent : DomainEventBase, INotification
{
    public decimal ControlPointId { get; }
    public string Description { get; }

    public ControlPointModifiedEvent(decimal controlPointId, string description)
    {
        ControlPointId = controlPointId;
        Description = description;
    }
}

public class ControlPointDeletedEvent : DomainEventBase, INotification
{
    public decimal ControlPointId { get; }

    public ControlPointDeletedEvent(decimal controlPointId) => ControlPointId = controlPointId;
}

// ── Control Point Request Domain Events ─────────────────────────────────────

public class ControlPointRequestCreatedEvent : DomainEventBase, INotification
{
    public decimal RequestId { get; }
    public decimal EmployeeSysId { get; }

    public ControlPointRequestCreatedEvent(decimal requestId, decimal employeeSysId)
    {
        RequestId = requestId;
        EmployeeSysId = employeeSysId;
    }
}

public class ControlPointRequestSubmittedEvent : DomainEventBase, INotification
{
    public decimal RequestId { get; }
    public decimal EmployeeSysId { get; }

    public ControlPointRequestSubmittedEvent(decimal requestId, decimal employeeSysId)
    {
        RequestId = requestId;
        EmployeeSysId = employeeSysId;
    }
}

public class ControlPointRequestApprovedEvent : DomainEventBase, INotification
{
    public decimal RequestId { get; }
    public decimal EmployeeSysId { get; }
    public decimal ApproverSysId { get; }

    public ControlPointRequestApprovedEvent(decimal requestId, decimal employeeSysId, decimal approverSysId)
    {
        RequestId = requestId;
        EmployeeSysId = employeeSysId;
        ApproverSysId = approverSysId;
    }
}

public class ControlPointRequestReturnedEvent : DomainEventBase, INotification
{
    public decimal RequestId { get; }
    public decimal EmployeeSysId { get; }
    public string Remarks { get; }

    public ControlPointRequestReturnedEvent(decimal requestId, decimal employeeSysId, string remarks)
    {
        RequestId = requestId;
        EmployeeSysId = employeeSysId;
        Remarks = remarks;
    }
}
