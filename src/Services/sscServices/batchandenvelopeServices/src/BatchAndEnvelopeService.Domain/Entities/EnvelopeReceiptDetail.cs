using BatchAndEnvelopeService.Domain.Common;

namespace BatchAndEnvelopeService.Domain.Entities;

public class EnvelopeReceiptDetail : Entity<long>
{
    public long EnvelopeId { get; private set; }
    public long DocumentId { get; private set; }
    public long UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }
    public string? EnvelopeType { get; private set; }
    public long? ScanLocationId { get; private set; }

    private EnvelopeReceiptDetail() { }

    public static EnvelopeReceiptDetail Create(long id, long envelopeId, long documentId, long updatedBy, string? envelopeType = null, long? scanLocationId = null)
    {
        return new EnvelopeReceiptDetail
        {
            Id = id,
            EnvelopeId = envelopeId,
            DocumentId = documentId,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow,
            EnvelopeType = envelopeType,
            ScanLocationId = scanLocationId
        };
    }
}
