using AimsTransactionService.Domain.Common;
using AimsTransactionService.Domain.Entities;
using AimsTransactionService.Domain.Enums;
using AimsTransactionService.Domain.Events;

namespace AimsTransactionService.Domain.Aggregates;

public class LeaveApplicationAggregate : AggregateRoot
{
    public long EmployeeSysId { get; private set; }
    public long LeaveId { get; private set; }
    public DateTime FromDate { get; private set; }
    public DateTime ToDate { get; private set; }
    public decimal LeaveDays { get; private set; }
    public string? Reason { get; private set; }
    public LeaveStatus Status { get; private set; }
    public DateTime AppliedOn { get; private set; }
    public long AppliedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public long? ApprovedBy { get; private set; }
    public string? Remarks { get; private set; }

    private readonly List<LeaveApproval> _approvals = [];
    public IReadOnlyCollection<LeaveApproval> Approvals => _approvals.AsReadOnly();

    private LeaveApplicationAggregate() { }

    public static LeaveApplicationAggregate Apply(
        long id,
        long employeeSysId,
        long leaveId,
        DateTime fromDate,
        DateTime toDate,
        decimal leaveDays,
        string? reason,
        long appliedBy)
    {
        if (fromDate > toDate)
            throw new InvalidOperationException("From date cannot be after To date.");

        var leave = new LeaveApplicationAggregate
        {
            Id = id,
            EmployeeSysId = employeeSysId,
            LeaveId = leaveId,
            FromDate = fromDate,
            ToDate = toDate,
            LeaveDays = leaveDays,
            Reason = reason,
            Status = LeaveStatus.Pending,
            AppliedOn = DateTime.UtcNow,
            AppliedBy = appliedBy
        };

        leave.AddDomainEvent(new LeaveAppliedEvent(id, employeeSysId, leaveId, fromDate, toDate, leaveDays));
        return leave;
    }

    public void Approve(long approvedBy, string? remarks)
    {
        Status = LeaveStatus.Approved;
        ApprovedOn = DateTime.UtcNow;
        ApprovedBy = approvedBy;
        Remarks = remarks;

        _approvals.Add(LeaveApproval.Create(0, Id, approvedBy));
        AddDomainEvent(new LeaveApprovedEvent(Id, EmployeeSysId, (char)(int)LeaveStatus.Approved, approvedBy));
    }

    public void Reject(long rejectedBy, string? remarks)
    {
        Status = LeaveStatus.Rejected;
        ApprovedOn = DateTime.UtcNow;
        ApprovedBy = rejectedBy;
        Remarks = remarks;
        AddDomainEvent(new LeaveApprovedEvent(Id, EmployeeSysId, (char)(int)LeaveStatus.Rejected, rejectedBy));
    }

    public void HydrateApprovals(IEnumerable<LeaveApproval> approvals)
    {
        _approvals.Clear();
        _approvals.AddRange(approvals);
    }
}
