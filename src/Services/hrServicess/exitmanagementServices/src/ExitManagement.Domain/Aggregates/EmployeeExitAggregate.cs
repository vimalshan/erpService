using ExitManagement.Domain.Common;
using ExitManagement.Domain.Entities;
using ExitManagement.Domain.Events;

namespace ExitManagement.Domain.Aggregates;

/// <summary>
/// Aggregate root that encapsulates the full lifecycle of an employee exit.
/// </summary>
public class EmployeeExitAggregate : AggregateRoot
{
    public EmployeeExit Exit { get; private set; } = null!;
    public List<ExitInterviewFeedback> InterviewFeedbacks { get; private set; } = new();
    public List<ExitResponsibilityMap> ResponsibilityMaps { get; private set; } = new();

    private EmployeeExitAggregate() { }

    public static EmployeeExitAggregate InitiateExit(
        decimal exitNo,
        decimal employeeSysId,
        decimal resignationId,
        string? resignationType,
        DateTime? expectedRelieveDate,
        string? remarks)
    {
        var aggregate = new EmployeeExitAggregate();
        aggregate.Exit = EmployeeExit.Create(exitNo, employeeSysId, resignationId,
            resignationType, expectedRelieveDate, remarks);

        aggregate.AddDomainEvent(new ExitInitiatedEvent(exitNo, employeeSysId, DateTime.UtcNow));
        return aggregate;
    }

    public void Approve(decimal approvedBy)
    {
        Exit.Approve(approvedBy);
        AddDomainEvent(new ExitApprovedEvent(Exit.ExitNo, Exit.EmployeeSysId, approvedBy, DateTime.UtcNow));
    }

    public void Revoke(string reason, decimal revokedBy)
    {
        Exit.Revoke(reason, revokedBy);
        AddDomainEvent(new ExitRevokedEvent(Exit.ExitNo, Exit.EmployeeSysId, reason, DateTime.UtcNow));
    }

    public void AddInterviewFeedback(decimal serialNo, string questionId, string feedback, decimal updatedBy)
    {
        var item = ExitInterviewFeedback.Create(Exit.ExitNo, serialNo, questionId, feedback, updatedBy);
        InterviewFeedbacks.Add(item);
    }

    public void AddResponsibilityMap(decimal ttId, decimal checklistMapId, string? primary, string? secondary, string? functionalHead)
    {
        var map = ExitResponsibilityMap.Create(ttId, Exit.EmployeeSysId, checklistMapId, primary, secondary, functionalHead);
        ResponsibilityMaps.Add(map);
    }
}
