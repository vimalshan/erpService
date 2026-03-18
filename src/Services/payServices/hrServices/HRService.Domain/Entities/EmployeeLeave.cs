namespace HRService.Domain.Entities;

public enum LeaveStatus
{
    Pending,
    Approved,
    Rejected,
    Cancelled
}

public class EmployeeLeave : Common.AggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public Guid LeaveTypeId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int NumberOfDays { get; private set; }
    public string? Reason { get; private set; }
    public LeaveStatus Status { get; private set; } = LeaveStatus.Pending;
    public Guid? ApprovedBy { get; private set; }
    public DateTime? ApprovalDate { get; private set; }

    private EmployeeLeave() { }

    public static EmployeeLeave Create(
        Guid employeeId,
        Guid leaveTypeId,
        DateTime startDate,
        DateTime endDate,
        string? reason = null)
    {
        if (employeeId == Guid.Empty)
            throw new ArgumentException("Employee id cannot be empty", nameof(employeeId));

        if (leaveTypeId == Guid.Empty)
            throw new ArgumentException("Leave type id cannot be empty", nameof(leaveTypeId));

        if (endDate < startDate)
            throw new ArgumentException("End date must be after start date");

        if (startDate < DateTime.Today)
            throw new ArgumentException("Start date cannot be in the past");

        var numberOfDays = (int)(endDate - startDate).TotalDays + 1;

        if (numberOfDays <= 0)
            throw new ArgumentException("Leave duration must be at least 1 day");

        return new EmployeeLeave
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            LeaveTypeId = leaveTypeId,
            StartDate = startDate,
            EndDate = endDate,
            NumberOfDays = numberOfDays,
            Reason = reason,
            Status = LeaveStatus.Pending,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
    }

    public void Approve(Guid approvedBy)
    {
        if (Status != LeaveStatus.Pending)
            throw new InvalidOperationException("Only pending leaves can be approved");

        Status = LeaveStatus.Approved;
        ApprovedBy = approvedBy;
        ApprovalDate = DateTime.UtcNow;
        ModifiedDate = DateTime.UtcNow;

        var @event = new Events.LeaveApprovedEvent
        {
            LeaveId = Id,
            EmployeeId = EmployeeId,
            StartDate = StartDate,
            EndDate = EndDate
        };

        AddDomainEvent(@event);
    }

    public void Reject()
    {
        if (Status != LeaveStatus.Pending)
            throw new InvalidOperationException("Only pending leaves can be rejected");

        Status = LeaveStatus.Rejected;
        ModifiedDate = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == LeaveStatus.Cancelled || Status == LeaveStatus.Rejected)
            throw new InvalidOperationException("Cannot cancel a rejected or already cancelled leave");

        Status = LeaveStatus.Cancelled;
        ModifiedDate = DateTime.UtcNow;
    }
}
