using WMTransactional.Domain.Common;
using WMTransactional.Domain.Events;

namespace WMTransactional.Domain.Entities;

public class Receiving : BaseEntity
{
    public int ReceivingId { get; private set; }
    public string ReceivingNumber { get; private set; } = null!;
    public int PoId { get; private set; }
    public DateTime ReceivedDate { get; private set; }
    public string Status { get; private set; } = null!;
    public string? Notes { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }

    private readonly List<ReceivingLine> _lines = [];
    public IReadOnlyCollection<ReceivingLine> Lines => _lines.AsReadOnly();

    public PurchaseOrder PurchaseOrder { get; private set; } = null!;

    private Receiving() { }

    public Receiving(string receivingNumber, int poId, string? notes, string? createdBy)
    {
        ReceivingNumber = receivingNumber;
        PoId = poId;
        ReceivedDate = DateTime.UtcNow;
        Status = "OPEN";
        Notes = notes;
        CreatedBy = createdBy;
        CreatedDate = DateTime.UtcNow;

        AddDomainEvent(new ReceivingCreatedEvent(receivingNumber, poId));
    }

    public void AddLine(int poLineId, int productId, int binId, decimal quantityReceived, string? lotNumber, DateTime? expiryDate, string? notes)
    {
        if (Status != "OPEN")
            throw new InvalidOperationException("Cannot add lines to a non-open receiving.");

        var line = new ReceivingLine(poLineId, productId, binId, quantityReceived, lotNumber, expiryDate, notes);
        _lines.Add(line);
    }

    public void Close()
    {
        if (Status != "OPEN")
            throw new InvalidOperationException("Only open receivings can be closed.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot close a receiving with no lines.");

        Status = "CLOSED";
        AddDomainEvent(new ReceivingClosedEvent(ReceivingNumber, PoId));
    }

    public void Cancel()
    {
        if (Status == "CLOSED" || Status == "CANCELLED")
            throw new InvalidOperationException($"Cannot cancel a {Status} receiving.");

        Status = "CANCELLED";
        AddDomainEvent(new ReceivingCancelledEvent(ReceivingNumber, PoId));
    }
}
