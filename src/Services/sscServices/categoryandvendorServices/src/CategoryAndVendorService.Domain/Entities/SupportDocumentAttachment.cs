using CategoryAndVendorService.Domain.Common;

namespace CategoryAndVendorService.Domain.Entities;

/// <summary>
/// Entity: Support Document Attachment (SUPDOC_ATT)
/// </summary>
public class SupportDocumentAttachment : Entity
{
    public long AttachmentId { get; private set; }
    public long DocId { get; private set; }
    public long InvoiceDocId { get; private set; }
    public char RefFlag { get; private set; }

    public SupportDocument SupportDocument { get; private set; } = null!;

    private SupportDocumentAttachment() { }

    public static SupportDocumentAttachment Create(long id, long docId, long invoiceDocId, char refFlag)
    {
        return new SupportDocumentAttachment
        {
            AttachmentId = id,
            DocId = docId,
            InvoiceDocId = invoiceDocId,
            RefFlag = refFlag
        };
    }
}
