using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentPoList : BaseEntity
{
    public long SeqId { get; private set; }
    public long DocId { get; private set; }
    public long PoId { get; private set; }
    public string PoNo { get; private set; } = null!;
    public string PoLineNo { get; private set; } = null!;
    public long? PoLineId { get; private set; }
    public DateTime? PoDate { get; private set; }
    public long? PoTermId { get; private set; }
    public long? PoTermSeqNo { get; private set; }

    public DocumentDetail Document { get; private set; } = null!;
}
