using SSCTransactional.Domain.Common;
using SSCTransactional.Domain.Events;

namespace SSCTransactional.Domain.Aggregates;

/// <summary>
/// AP Allocation aggregate root — maps to DOC_APALLDET.
/// Manages document allocation to AP groups for processing, validation, and payments.
/// </summary>
public class AllocationAggregate : AggregateRoot<long>
{
    public long DocId { get; private set; }
    public string Action { get; private set; } = default!;      // M/C/P
    public long GroupId { get; private set; }
    public string PullStatus { get; private set; } = "N";       // Y/N
    public long PullUserId { get; private set; }
    public int Priority { get; private set; }
    public long AllocatedBy { get; private set; }
    public DateTime AllocatedOn { get; private set; }
    public string? Remarks { get; private set; }
    public string ActionFlag { get; private set; } = "N";       // N/H/D/F/C/P/R/S/E/B
    public DateTime? ActionDate { get; private set; }
    public long? CorrespondenceId { get; private set; }
    public long? DefectType { get; private set; }
    public string? CloseRemarks { get; private set; }
    public long ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }
    public DateTime PulledOn { get; private set; }

    private readonly List<DefectiveAttachment> _defectiveAttachments = new();
    public IReadOnlyCollection<DefectiveAttachment> DefectiveAttachments => _defectiveAttachments.AsReadOnly();

    private AllocationAggregate() { }

    public static AllocationAggregate Create(
        long id, long docId, string action, long groupId, int priority, long allocatedBy)
    {
        var allocation = new AllocationAggregate
        {
            Id = id,
            DocId = docId,
            Action = action,
            GroupId = groupId,
            PullStatus = "N",
            PullUserId = 0,
            Priority = priority,
            AllocatedBy = allocatedBy,
            AllocatedOn = DateTime.UtcNow,
            ActionFlag = "N",
            ModifiedBy = allocatedBy,
            ModifiedOn = DateTime.UtcNow,
            PulledOn = DateTime.UtcNow
        };

        allocation.RaiseDomainEvent(new AllocationCreatedDomainEvent(id, docId, action, groupId));
        return allocation;
    }

    public void Pull(long userId)
    {
        if (PullStatus == "Y")
            throw new Exceptions.TransactionDomainException($"Allocation {Id} is already pulled.");

        PullStatus = "Y";
        PullUserId = userId;
        PulledOn = DateTime.UtcNow;
        ModifiedBy = userId;
        ModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new AllocationPulledDomainEvent(Id, userId));
    }

    public void Complete(long userId, string? closeRemarks = null)
    {
        if (ActionFlag == "C")
            throw new Exceptions.TransactionDomainException($"Allocation {Id} is already completed.");

        ActionFlag = "C";
        ActionDate = DateTime.UtcNow;
        CloseRemarks = closeRemarks;
        ModifiedBy = userId;
        ModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new AllocationCompletedDomainEvent(Id, DocId));
    }

    public void SetHold(long userId, long correspondenceId, string? remarks = null)
    {
        ActionFlag = "H";
        CorrespondenceId = correspondenceId;
        Remarks = remarks;
        ModifiedBy = userId;
        ModifiedOn = DateTime.UtcNow;
    }

    public void ReleaseHold(long userId)
    {
        ActionFlag = "R";
        ModifiedBy = userId;
        ModifiedOn = DateTime.UtcNow;
    }

    public void MarkDefective(long userId, long defectType, string? remarks = null)
    {
        ActionFlag = "D";
        DefectType = defectType;
        Remarks = remarks;
        ModifiedBy = userId;
        ModifiedOn = DateTime.UtcNow;
    }

    public void ForwardToGroup(long userId, string? remarks = null)
    {
        ActionFlag = "F";
        Remarks = remarks;
        ModifiedBy = userId;
        ModifiedOn = DateTime.UtcNow;
    }

    public void SendForRescan(long userId)
    {
        ActionFlag = "S";
        ModifiedBy = userId;
        ModifiedOn = DateTime.UtcNow;
    }

    public void Reject(long userId, string? remarks = null)
    {
        ActionFlag = "E";
        CloseRemarks = remarks;
        ModifiedBy = userId;
        ModifiedOn = DateTime.UtcNow;
    }

    public void SendBack(long userId, string? remarks = null)
    {
        ActionFlag = "B";
        Remarks = remarks;
        ModifiedBy = userId;
        ModifiedOn = DateTime.UtcNow;
    }

    public void AddDefectiveAttachment(DefectiveAttachment attachment)
    {
        _defectiveAttachments.Add(attachment);
    }
}
