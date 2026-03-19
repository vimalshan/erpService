using BatchAndEnvelopeService.Domain.Common;

namespace BatchAndEnvelopeService.Domain.Entities;

public class BatchReceiptDetail : Entity<long>
{
    public long BatchId { get; private set; }
    public long EnvelopeId { get; private set; }
    public long UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }
    public long? ScanLocationId { get; private set; }

    private BatchReceiptDetail() { }

    public static BatchReceiptDetail Create(long id, long batchId, long envelopeId, long updatedBy, long? scanLocationId = null)
    {
        return new BatchReceiptDetail
        {
            Id = id,
            BatchId = batchId,
            EnvelopeId = envelopeId,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow,
            ScanLocationId = scanLocationId
        };
    }
}
