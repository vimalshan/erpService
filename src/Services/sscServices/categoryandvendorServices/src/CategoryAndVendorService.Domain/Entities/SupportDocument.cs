using CategoryAndVendorService.Domain.Common;

namespace CategoryAndVendorService.Domain.Entities;

/// <summary>
/// Aggregate Root: Support Document Details (SUPDOC_DET)
/// </summary>
public class SupportDocument : Entity
{
    public long DocId { get; private set; }
    public long DocCategory { get; private set; }
    public long InvoiceDocId { get; private set; }
    public string? DocKey { get; private set; }
    public string DocStatus { get; private set; } = null!;
    public string? PbgNo { get; private set; }
    public DateTime? PbgStart { get; private set; }
    public DateTime? PbgExpDate { get; private set; }
    public long? Amount { get; private set; }
    public long? RecDue { get; private set; }

    private readonly List<SupportDocumentAttachment> _attachments = new();
    public IReadOnlyCollection<SupportDocumentAttachment> Attachments => _attachments.AsReadOnly();

    private SupportDocument() { }

    public static SupportDocument Create(long id, long docCategory, long invoiceDocId,
        string docStatus, string? docKey = null, string? pbgNo = null,
        DateTime? pbgStart = null, DateTime? pbgExpDate = null,
        long? amount = null, long? recDue = null)
    {
        return new SupportDocument
        {
            DocId = id,
            DocCategory = docCategory,
            InvoiceDocId = invoiceDocId,
            DocKey = docKey,
            DocStatus = docStatus,
            PbgNo = pbgNo,
            PbgStart = pbgStart,
            PbgExpDate = pbgExpDate,
            Amount = amount,
            RecDue = recDue
        };
    }

    public void UpdateStatus(string status) => DocStatus = status;

    public void AddAttachment(SupportDocumentAttachment attachment) => _attachments.Add(attachment);
}
