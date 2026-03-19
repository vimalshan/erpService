using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentCorrespondence : BaseEntity
{
    public long CorrId { get; private set; }
    public long DocId { get; private set; }
    public long AllocationId { get; private set; }
    public long HoldCategory { get; private set; }
    public long HoldType { get; private set; }
    public DateTime HoldDate { get; private set; }
    public string HoldRemarks { get; private set; } = null!;
    public long HoldBy { get; private set; }
    public string HoldStatus { get; private set; } = null!;
    public DateTime? ReleaseDate { get; private set; }
    public string? ReleaseRemarks { get; private set; }
    public long? ReleaseBy { get; private set; }
    public decimal? HoldNature { get; private set; }

    public DocumentDetail Document { get; private set; } = null!;
    public ICollection<DocumentCorrespondenceAttachment> Attachments { get; private set; } = [];
}
