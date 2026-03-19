using BatchAndEnvelopeService.Domain.Common;
using BatchAndEnvelopeService.Domain.Entities;
using BatchAndEnvelopeService.Domain.Events;
using BatchAndEnvelopeService.Domain.Exceptions;

namespace BatchAndEnvelopeService.Domain.Aggregates;

public class EnvelopeAggregate : AggregateRoot<long>
{
    public string EnvelopeType { get; private set; } = default!;
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? ReceivedBy { get; private set; }
    public DateTime? ReceivedOn { get; private set; }
    public string SummaryFlag { get; private set; } = default!;
    public long? CancelledBy { get; private set; }
    public DateTime? CancelledOn { get; private set; }
    public long? ConfirmedBy { get; private set; }
    public DateTime? ConfirmedOn { get; private set; }
    public long? ScanLotNo { get; private set; }
    public long LocationId { get; private set; }

    private readonly List<EnvelopeDetail> _details = new();
    public IReadOnlyCollection<EnvelopeDetail> Details => _details.AsReadOnly();

    private readonly List<EnvelopeReceiptDetail> _receiptDetails = new();
    public IReadOnlyCollection<EnvelopeReceiptDetail> ReceiptDetails => _receiptDetails.AsReadOnly();

    private EnvelopeAggregate() { }

    public static EnvelopeAggregate Create(long id, string envelopeType, long createdBy, long locationId)
    {
        var envelope = new EnvelopeAggregate
        {
            Id = id,
            EnvelopeType = envelopeType,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow,
            SummaryFlag = "N",
            LocationId = locationId
        };
        envelope.RaiseDomainEvent(new EnvelopeCreatedDomainEvent(id, envelopeType, createdBy, locationId));
        return envelope;
    }

    public void AddDetail(EnvelopeDetail detail) => _details.Add(detail);

    public void Confirm(long confirmedBy)
    {
        if (CancelledBy.HasValue)
            throw new EnvelopeDomainException($"Envelope {Id} is already cancelled.");
        if (ConfirmedBy.HasValue)
            throw new EnvelopeDomainException($"Envelope {Id} is already confirmed.");

        ConfirmedBy = confirmedBy;
        ConfirmedOn = DateTime.UtcNow;
        SummaryFlag = "Y";
        RaiseDomainEvent(new EnvelopeConfirmedDomainEvent(Id, confirmedBy));
    }

    public void Cancel(long cancelledBy)
    {
        if (ConfirmedBy.HasValue)
            throw new EnvelopeDomainException($"Envelope {Id} is already confirmed and cannot be cancelled.");

        CancelledBy = cancelledBy;
        CancelledOn = DateTime.UtcNow;
        RaiseDomainEvent(new EnvelopeCancelledDomainEvent(Id, cancelledBy));
    }

    public void AssignScanLot(long scanLotNo) => ScanLotNo = scanLotNo;

    public void MarkReceived(long receivedBy)
    {
        ReceivedBy = receivedBy;
        ReceivedOn = DateTime.UtcNow;
    }

    public void AddReceiptDetail(EnvelopeReceiptDetail receiptDetail) => _receiptDetails.Add(receiptDetail);
}
