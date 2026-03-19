using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentMrcList : BaseEntity
{
    public long SeqId { get; private set; }
    public long DocId { get; private set; }
    public long LineId { get; private set; }
    public long MrcId { get; private set; }
    public string MrcNo { get; private set; } = null!;
    public DateTime? MrcDate { get; private set; }
    public long? PoLineId { get; private set; }

    public DocumentDetail Document { get; private set; } = null!;
}
