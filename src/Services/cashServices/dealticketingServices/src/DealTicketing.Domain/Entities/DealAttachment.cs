using DealTicketing.Domain.Common;

namespace DealTicketing.Domain.Entities;

public class DealAttachment : BaseEntity
{
    public long DealAttachmentId { get; private set; }
    public long DealBatchId { get; private set; }
    public long DealId { get; private set; }
    public string DealAttachmentType { get; private set; } = default!;
    public string DealAttachmentFile { get; private set; } = default!;

    public DealDetail DealDetail { get; private set; } = default!;
    public DealBatch DealBatch { get; private set; } = default!;

    private DealAttachment() { }

    public DealAttachment(long attachmentId, long batchId, long dealId, string type, string file)
    {
        DealAttachmentId = attachmentId;
        DealBatchId = batchId;
        DealId = dealId;
        DealAttachmentType = type;
        DealAttachmentFile = file;
    }
}

public class DealSettlementAttachment : BaseEntity
{
    public long DealAttachmentId { get; private set; }
    public long DealSetId { get; private set; }
    public string DealAttachmentType { get; private set; } = default!;
    public string DealAttachmentFile { get; private set; } = default!;

    public DealSettlement DealSettlement { get; private set; } = default!;

    private DealSettlementAttachment() { }

    public DealSettlementAttachment(long attachmentId, long setId, string type, string file)
    {
        DealAttachmentId = attachmentId;
        DealSetId = setId;
        DealAttachmentType = type;
        DealAttachmentFile = file;
    }
}
