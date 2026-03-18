namespace ApprovalService.Domain.Entities;

using ApprovalService.Domain.Common;
using ApprovalService.Domain.Events;

/// <summary>
/// Represents an Approver Employee - maps employees to approval processes
/// </summary>
public class ApproverEmployee : Entity
{
    public long ApprovalMasterId { get; private set; }
    public long EmployeeSysId { get; private set; }
    public int ApproverLevel { get; private set; }
    public ApproverStatus Status { get; private set; } = ApproverStatus.Active;
    public DateTime EffectiveFrom { get; private set; }
    public DateTime? EffectiveTo { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private ApproverEmployee() { }

    /// <summary>
    /// Creates a new approver employee assignment
    /// </summary>
    public static ApproverEmployee Create(
        long approvalMasterId,
        long employeeSysId,
        int approverLevel,
        DateTime effectiveFrom,
        DateTime? effectiveTo,
        long createdBy)
    {
        var approver = new ApproverEmployee
        {
            ApprovalMasterId = approvalMasterId,
            EmployeeSysId = employeeSysId,
            ApproverLevel = approverLevel,
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow,
            Status = ApproverStatus.Active
        };

        approver.AddDomainEvent(new ApproverEmployeeCreatedEvent(
            approver.Id,
            approvalMasterId,
            employeeSysId,
            approverLevel,
            effectiveFrom));

        return approver;
    }

    /// <summary>
    /// Updates the approver assignment
    /// </summary>
    public void Update(int approverLevel, DateTime? effectiveTo, long updatedBy)
    {
        ApproverLevel = approverLevel;
        EffectiveTo = effectiveTo;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new ApproverEmployeeUpdatedEvent(Id, ApprovalMasterId, EmployeeSysId));
    }

    /// <summary>
    /// Updates the status
    /// </summary>
    public void UpdateStatus(ApproverStatus status, long updatedBy)
    {
        if (Status != status)
        {
            Status = status;
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;

            AddDomainEvent(new ApproverEmployeeStatusChangedEvent(Id, ApprovalMasterId, EmployeeSysId, Status));
        }
    }

    /// <summary>
    /// Deactivates the approver
    /// </summary>
    public void Deactivate(long updatedBy)
    {
        UpdateStatus(ApproverStatus.Inactive, updatedBy);
    }

    /// <summary>
    /// Activates the approver
    /// </summary>
    public void Activate(long updatedBy)
    {
        UpdateStatus(ApproverStatus.Active, updatedBy);
    }

    /// <summary>
    /// Checks if the approver is currently active (considering effective dates)
    /// </summary>
    public bool IsCurrentlyActive()
    {
        var now = DateTime.UtcNow.Date;
        return Status == ApproverStatus.Active
            && EffectiveFrom <= now
            && (EffectiveTo == null || EffectiveTo >= now);
    }
}

/// <summary>
/// Enumeration for approver status
/// </summary>
public enum ApproverStatus
{
    Active = 1,
    Inactive = 2
}
