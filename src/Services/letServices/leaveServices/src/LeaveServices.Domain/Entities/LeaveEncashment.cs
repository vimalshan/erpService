using LeaveServices.Domain.Common;
using LeaveServices.Domain.Events;
using LeaveServices.Domain.ValueObjects;

namespace LeaveServices.Domain.Entities;

/// <summary>
/// Aggregate Root: Leave Encashment (maps to LEAVE_ENCASHMENT)
/// </summary>
public sealed class LeaveEncashment : BaseEntity
{
    public long EncashmentId { get; private set; }
    public long EmpSysId { get; private set; }
    public string LeaveType { get; private set; } = default!;
    public int EncashmentDays { get; private set; }
    public decimal EncashmentAmount { get; private set; }
    public DateOnly RequestDate { get; private set; }
    public char EncashmentStatusCode { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? ModifiedOn { get; private set; }
    public long? ModifiedBy { get; private set; }

    // Mapped to database column ENCASHMENT_STATUS via EF config
    public char EncashmentStatus => EncashmentStatusCode;

    private LeaveEncashment() { }

    public static LeaveEncashment Create(
        long empSysId,
        string leaveType,
        int encashmentDays,
        decimal encashmentAmount,
        DateOnly requestDate,
        long createdBy)
    {
        if (encashmentDays <= 0)
            throw new ArgumentOutOfRangeException(nameof(encashmentDays), "Encashment days must be positive.");
        if (encashmentAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(encashmentAmount), "Encashment amount cannot be negative.");

        var entity = new LeaveEncashment
        {
            EmpSysId = empSysId,
            LeaveType = leaveType,
            EncashmentDays = encashmentDays,
            EncashmentAmount = encashmentAmount,
            RequestDate = requestDate,
            EncashmentStatusCode = ValueObjects.EncashmentStatus.Pending.Code,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        entity.RaiseDomainEvent(new LeaveEncashmentRequestedEvent(
            0, empSysId, leaveType, encashmentDays, encashmentAmount));
        return entity;
    }

    public void UpdateStatus(char newStatus, long modifiedBy)
    {
        var oldStatus = EncashmentStatusCode;
        EncashmentStatusCode = newStatus;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new LeaveEncashmentStatusChangedEvent(
            EncashmentId, oldStatus, newStatus, modifiedBy));
    }
}
