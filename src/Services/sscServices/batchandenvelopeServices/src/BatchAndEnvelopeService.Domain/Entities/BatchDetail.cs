using BatchAndEnvelopeService.Domain.Common;

namespace BatchAndEnvelopeService.Domain.Entities;

public class BatchDetail : Entity<int>
{
    public long BatchId { get; private set; }
    public int EnvelopeId { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public string ReceiveFlag { get; private set; } = default!;
    public long? ReceivedBy { get; private set; }
    public DateTime? ReceivedOn { get; private set; }
    public DateTime CancelDate { get; private set; }
    public long CancelBy { get; private set; }

    private BatchDetail() { }

    public static BatchDetail Create(int id, long batchId, int envelopeId, long createdBy)
    {
        return new BatchDetail
        {
            Id = id,
            BatchId = batchId,
            EnvelopeId = envelopeId,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow,
            ReceiveFlag = "N",
            CancelDate = DateTime.UtcNow,
            CancelBy = 0
        };
    }

    public void MarkReceived(long receivedBy)
    {
        ReceiveFlag = "Y";
        ReceivedBy = receivedBy;
        ReceivedOn = DateTime.UtcNow;
    }

    public void Cancel(long cancelledBy)
    {
        CancelBy = cancelledBy;
        CancelDate = DateTime.UtcNow;
    }
}
