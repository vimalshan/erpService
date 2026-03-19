using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentApAllocation : BaseEntity
{
    public long AllocationId { get; private set; }
    public long DocId { get; private set; }
    public string Action { get; private set; } = null!;
    public long GroupId { get; private set; }
    public string PullStatus { get; private set; } = null!;
    public long PullUserId { get; private set; }
    public int Priority { get; private set; }
    public long AllocatedBy { get; private set; }
    public DateTime AllocatedOn { get; private set; }
    public string? Remarks { get; private set; }
    public string ActionFlag { get; private set; } = null!;
    public DateTime? ActionDate { get; private set; }
    public long? CorrespondenceId { get; private set; }
    public long? DefectType { get; private set; }
    public string? CloseRemarks { get; private set; }
    public long ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }
    public DateTime PulledOn { get; private set; }

    public DocumentDetail Document { get; private set; } = null!;
}
