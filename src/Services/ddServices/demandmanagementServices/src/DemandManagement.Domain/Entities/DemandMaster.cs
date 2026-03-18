using DemandManagement.Domain.Common;
using DemandManagement.Domain.Events;

namespace DemandManagement.Domain.Entities;

public class DemandMaster : AggregateRoot
{
    public long DemandId { get; set; }
    public string DemandType { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public string DemandDescription { get; set; } = string.Empty;
    public DateTime? RequiredDate { get; set; }
    public string Priority { get; set; } = string.Empty;
    public string DemandStatus { get; set; } = "O";
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string? ApprovalRemarks { get; set; }
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string? CompletionRemarks { get; set; }
    public long? CompletedBy { get; set; }
    public DateTime? CompletionDate { get; set; }

    public void Approve(long approvedBy, string remarks)
    {
        DemandStatus = "A";
        ApprovedBy = approvedBy;
        ApprovalRemarks = remarks;
        ApprovalDate = DateTime.UtcNow;
        RaiseDomainEvent(new DemandApprovedEvent(DemandId, approvedBy));
    }

    public void Reject(long rejectedBy, string remarks)
    {
        DemandStatus = "R";
        ApprovedBy = rejectedBy;
        ApprovalRemarks = remarks;
        ApprovalDate = DateTime.UtcNow;
        RaiseDomainEvent(new DemandRejectedEvent(DemandId, rejectedBy));
    }

    public void Complete(long completedBy, string remarks)
    {
        DemandStatus = "C";
        CompletedBy = completedBy;
        CompletionRemarks = remarks;
        CompletionDate = DateTime.UtcNow;
        RaiseDomainEvent(new DemandCompletedEvent(DemandId, completedBy));
    }
}
