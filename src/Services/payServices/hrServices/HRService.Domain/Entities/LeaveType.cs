namespace HRService.Domain.Entities;

public class LeaveType : Common.AggregateRoot
{
    public string LeaveTypeName { get; private set; } = null!;
    public int? MaxDaysPerYear { get; private set; }
    public bool IsPaid { get; private set; } = true;

    private LeaveType() { }

    public static LeaveType Create(string leaveTypeName, int? maxDaysPerYear = null, bool isPaid = true)
    {
        if (string.IsNullOrWhiteSpace(leaveTypeName))
            throw new ArgumentException("Leave type name cannot be empty", nameof(leaveTypeName));

        return new LeaveType
        {
            Id = Guid.NewGuid(),
            LeaveTypeName = leaveTypeName,
            MaxDaysPerYear = maxDaysPerYear,
            IsPaid = isPaid,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
    }
}
