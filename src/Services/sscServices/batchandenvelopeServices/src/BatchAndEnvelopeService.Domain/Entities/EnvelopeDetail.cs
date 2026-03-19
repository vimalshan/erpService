using BatchAndEnvelopeService.Domain.Common;

namespace BatchAndEnvelopeService.Domain.Entities;

public class EnvelopeDetail : Entity<long>
{
    public long EnvelopeId { get; private set; }
    public string EnvelopeType { get; private set; } = default!;
    public int DocumentId { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public string ReceiveFlag { get; private set; } = default!;
    public long ReceivedBy { get; private set; }
    public DateTime ReceivedOn { get; private set; }
    public DateTime CancelDate { get; private set; }
    public long CancelBy { get; private set; }

    private EnvelopeDetail() { }

    public static EnvelopeDetail Create(long id, long envelopeId, string envelopeType, int documentId, long createdBy)
    {
        return new EnvelopeDetail
        {
            Id = id,
            EnvelopeId = envelopeId,
            EnvelopeType = envelopeType,
            DocumentId = documentId,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow,
            ReceiveFlag = "N",
            ReceivedBy = 0,
            ReceivedOn = DateTime.MinValue,
            CancelDate = DateTime.MinValue,
            CancelBy = 0
        };
    }

    public void MarkReceived(long receivedBy)
    {
        ReceiveFlag = "Y";
        ReceivedBy = receivedBy;
        ReceivedOn = DateTime.UtcNow;
    }
}
