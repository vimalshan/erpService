namespace ApprovalService.Domain.Entities;

using ApprovalService.Domain.Common;
using ApprovalService.Domain.Events;
using ApprovalService.Domain.ValueObjects;

/// <summary>
/// Represents an Approval Master - the approval process definition
/// </summary>
public class ApprovalMaster : Entity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Module { get; private set; } = string.Empty; // PER, DDP, LET
    public ApprovalStatus Status { get; private set; } = ApprovalStatus.Active;
    public int Level { get; private set; } = 1;
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    // Navigation property
    private readonly List<ApproverEmployee> _approvers = [];
    public IReadOnlyCollection<ApproverEmployee> Approvers => _approvers.AsReadOnly();

    private ApprovalMaster() { }

    /// <summary>
    /// Creates a new approval master
    /// </summary>
    public static ApprovalMaster Create(
        string code,
        string name,
        string module,
        int level,
        long createdBy)
    {
        var approval = new ApprovalMaster
        {
            Code = code,
            Name = name,
            Module = module,
            Level = level,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow,
            Status = ApprovalStatus.Active
        };

        approval.AddDomainEvent(new ApprovalMasterCreatedEvent(
            approval.Id,
            approval.Code,
            approval.Name,
            approval.Module,
            approval.Level));

        return approval;
    }

    /// <summary>
    /// Updates approval master details
    /// </summary>
    public void Update(string name, int level, long updatedBy)
    {
        if (Name != name || Level != level)
        {
            Name = name;
            Level = level;
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;

            AddDomainEvent(new ApprovalMasterUpdatedEvent(Id, Code, Name, Level));
        }
    }

    /// <summary>
    /// Updates status
    /// </summary>
    public void UpdateStatus(ApprovalStatus status, long updatedBy)
    {
        if (Status != status)
        {
            Status = status;
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;

            AddDomainEvent(new ApprovalMasterStatusChangedEvent(Id, Code, Status));
        }
    }

    /// <summary>
    /// Deactivates this approval process
    /// </summary>
    public void Deactivate(long updatedBy)
    {
        UpdateStatus(ApprovalStatus.Inactive, updatedBy);
    }

    /// <summary>
    /// Activates this approval process
    /// </summary>
    public void Activate(long updatedBy)
    {
        UpdateStatus(ApprovalStatus.Active, updatedBy);
    }

    /// <summary>
    /// Assigns an approver
    /// </summary>
    public void AssignApprover(ApproverEmployee approver)
    {
        _approvers.Add(approver);
        AddDomainEvent(new ApproverAssignedEvent(
            Id,
            Code,
            approver.Id,
            approver.EmployeeSysId,
            approver.ApproverLevel));
    }

    /// <summary>
    /// Removes an approver
    /// </summary>
    public void RemoveApprover(ApproverEmployee approver)
    {
        _approvers.Remove(approver);
        AddDomainEvent(new ApproverRemovedEvent(
            Id,
            Code,
            approver.Id,
            approver.EmployeeSysId));
    }
}
