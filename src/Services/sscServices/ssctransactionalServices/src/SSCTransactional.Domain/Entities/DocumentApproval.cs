using SSCTransactional.Domain.Common;
using SSCTransactional.Domain.Events;

namespace SSCTransactional.Domain.Entities;

/// <summary>Maps to DOC_APPDET — Document approval records</summary>
public class DocumentApproval : Entity<long>
{
    public long DocId { get; private set; }
    public long ApproverUserId { get; private set; }
    public string Status { get; private set; } = default!;      // single char status
    public string? Remarks { get; private set; }
    public DateTime ApprovalDate { get; private set; }

    private DocumentApproval() { }

    public static DocumentApproval Create(long id, long docId, long approverUserId, string status, DateTime approvalDate, string? remarks = null)
    {
        var approval = new DocumentApproval
        {
            Id = id,
            DocId = docId,
            ApproverUserId = approverUserId,
            Status = status,
            Remarks = remarks,
            ApprovalDate = approvalDate
        };

        approval.RaiseDomainEvent(new ApprovalCreatedDomainEvent(id, docId, approverUserId));
        return approval;
    }

    public void UpdateStatus(string status, string? remarks = null)
    {
        Status = status;
        Remarks = remarks;
    }
}
