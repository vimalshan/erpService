using TimesheetService.Domain.Common;
using TimesheetService.Domain.Events;
using TimesheetService.Domain.ValueObjects;
using TimesheetService.Domain.Exceptions;

namespace TimesheetService.Domain.Entities;

/// <summary>
/// Aggregate root representing a single employee timesheet entry.
/// Maps to TSE_TIMESHEET table.
/// </summary>
public sealed class Timesheet : BaseEntity
{
    // Private constructor for EF Core
    private Timesheet() { }

    public long TimesheetId { get; private set; }
    public long EmployeeId { get; private set; }
    public DateOnly TimesheetDate { get; private set; }
    public DateOnly WorkDate { get; private set; }
    public TimeOnly? StartTime { get; private set; }
    public TimeOnly? EndTime { get; private set; }
    public decimal? TotalHours { get; private set; }
    public long? ProjectId { get; private set; }
    public long? TaskId { get; private set; }
    public string? WorkDescription { get; private set; }
    public DateTime RecordedDate { get; private set; }
    public TimesheetStatus Status { get; private set; } = TimesheetStatus.Draft;
    public ApprovalStatus ApprovalStatus { get; private set; } = ApprovalStatus.Pending;
    public long? ApprovedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public string? RejectionReason { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    public static Timesheet Create(
        long employeeId,
        DateOnly timesheetDate,
        DateOnly workDate,
        TimeOnly? startTime,
        TimeOnly? endTime,
        decimal? totalHours,
        long? projectId,
        long? taskId,
        string? workDescription,
        long createdBy)
    {
        if (employeeId <= 0)
            throw new TimesheetDomainException("EmployeeId must be positive.");
        if (workDate > timesheetDate)
            throw new TimesheetDomainException("WorkDate cannot be after TimesheetDate.");

        var timesheet = new Timesheet
        {
            EmployeeId      = employeeId,
            TimesheetDate   = timesheetDate,
            WorkDate        = workDate,
            StartTime       = startTime,
            EndTime         = endTime,
            TotalHours      = totalHours,
            ProjectId       = projectId,
            TaskId          = taskId,
            WorkDescription = workDescription,
            RecordedDate    = DateTime.UtcNow,
            Status          = TimesheetStatus.Draft,
            ApprovalStatus  = ApprovalStatus.Pending,
            CreatedBy       = createdBy,
            CreatedOn       = DateTime.UtcNow
        };

        timesheet.RaiseDomainEvent(new TimesheetCreatedEvent(timesheet.TimesheetId, employeeId));
        return timesheet;
    }

    public void Submit(long updatedBy)
    {
        if (Status != TimesheetStatus.Draft)
            throw new TimesheetDomainException($"Only DRAFT timesheets can be submitted. Current status: {Status}");

        Status    = TimesheetStatus.Submitted;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        RaiseDomainEvent(new TimesheetSubmittedEvent(TimesheetId, EmployeeId));
    }

    public void Approve(long approverId)
    {
        if (Status != TimesheetStatus.Submitted)
            throw new TimesheetDomainException($"Only SUBMITTED timesheets can be approved. Current status: {Status}");

        Status         = TimesheetStatus.Approved;
        ApprovalStatus = ApprovalStatus.Approved;
        ApprovedBy     = approverId;
        ApprovedOn     = DateTime.UtcNow;
        UpdatedBy      = approverId;
        UpdatedOn      = DateTime.UtcNow;

        RaiseDomainEvent(new TimesheetApprovedEvent(TimesheetId, EmployeeId, approverId));
    }

    public void Reject(long approverId, string rejectionReason)
    {
        if (Status != TimesheetStatus.Submitted)
            throw new TimesheetDomainException($"Only SUBMITTED timesheets can be rejected. Current status: {Status}");
        if (string.IsNullOrWhiteSpace(rejectionReason))
            throw new TimesheetDomainException("Rejection reason is required.");

        Status          = TimesheetStatus.Rejected;
        ApprovalStatus  = ApprovalStatus.Rejected;
        RejectionReason = rejectionReason;
        UpdatedBy       = approverId;
        UpdatedOn       = DateTime.UtcNow;

        RaiseDomainEvent(new TimesheetRejectedEvent(TimesheetId, EmployeeId, rejectionReason));
    }

    public void Update(
        TimeOnly? startTime,
        TimeOnly? endTime,
        decimal? totalHours,
        long? projectId,
        long? taskId,
        string? workDescription,
        long updatedBy)
    {
        if (Status != TimesheetStatus.Draft)
            throw new TimesheetDomainException("Only DRAFT timesheets can be updated.");

        StartTime       = startTime;
        EndTime         = endTime;
        TotalHours      = totalHours;
        ProjectId       = projectId;
        TaskId          = taskId;
        WorkDescription = workDescription;
        UpdatedBy       = updatedBy;
        UpdatedOn       = DateTime.UtcNow;
    }
}
