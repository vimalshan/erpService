using BatchAndEnvelopeService.Domain.Common;
using BatchAndEnvelopeService.Domain.Entities;
using BatchAndEnvelopeService.Domain.Events;
using BatchAndEnvelopeService.Domain.Exceptions;

namespace BatchAndEnvelopeService.Domain.Aggregates;

public class BatchAggregate : AggregateRoot<long>
{
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long LocationId { get; private set; }
    public long ReceivedBy { get; private set; }
    public DateTime ReceivedOn { get; private set; }
    public string PodNo { get; private set; } = default!;
    public string SummaryFlag { get; private set; } = default!;
    public long? CancelBy { get; private set; }
    public DateTime? CancelDate { get; private set; }
    public long? ConfirmedBy { get; private set; }
    public DateTime? ConfirmedOn { get; private set; }
    public string? CourierName { get; private set; }
    public string ScanFlag { get; private set; } = default!;

    private readonly List<BatchDetail> _details = new();
    public IReadOnlyCollection<BatchDetail> Details => _details.AsReadOnly();

    private readonly List<BatchReceiptDetail> _receiptDetails = new();
    public IReadOnlyCollection<BatchReceiptDetail> ReceiptDetails => _receiptDetails.AsReadOnly();

    private BatchAggregate() { }

    public static BatchAggregate Create(long id, long createdBy, long locationId, long receivedBy, string podNo, string? courierName = null)
    {
        var batch = new BatchAggregate
        {
            Id = id,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow,
            LocationId = locationId,
            ReceivedBy = receivedBy,
            ReceivedOn = DateTime.UtcNow,
            PodNo = podNo,
            SummaryFlag = "N",
            ScanFlag = "PENDING",
            CourierName = courierName
        };
        batch.RaiseDomainEvent(new BatchCreatedDomainEvent(id, createdBy, locationId));
        return batch;
    }

    public void AddDetail(BatchDetail detail)
    {
        _details.Add(detail);
    }

    public void Confirm(long confirmedBy)
    {
        if (CancelBy.HasValue)
            throw new BatchDomainException($"Batch {Id} is already cancelled.");
        if (ConfirmedBy.HasValue)
            throw new BatchDomainException($"Batch {Id} is already confirmed.");

        ConfirmedBy = confirmedBy;
        ConfirmedOn = DateTime.UtcNow;
        SummaryFlag = "Y";
        RaiseDomainEvent(new BatchConfirmedDomainEvent(Id, confirmedBy));
    }

    public void Cancel(long cancelledBy)
    {
        if (ConfirmedBy.HasValue)
            throw new BatchDomainException($"Batch {Id} is already confirmed and cannot be cancelled.");

        CancelBy = cancelledBy;
        CancelDate = DateTime.UtcNow;
        RaiseDomainEvent(new BatchCancelledDomainEvent(Id, cancelledBy));
    }

    public void UpdateScanFlag(string scanFlag) => ScanFlag = scanFlag;

    public void AddReceiptDetail(BatchReceiptDetail receiptDetail) => _receiptDetails.Add(receiptDetail);
}
