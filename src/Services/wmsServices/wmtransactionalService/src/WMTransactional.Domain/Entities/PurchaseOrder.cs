using WMTransactional.Domain.Common;
using WMTransactional.Domain.Events;

namespace WMTransactional.Domain.Entities;

public class PurchaseOrder : BaseEntity
{
    public int PoId { get; private set; }
    public string PoNumber { get; private set; } = null!;
    public int SupplierId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? ExpectedDate { get; private set; }
    public string Status { get; private set; } = null!;
    public string? Notes { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime ModifiedDate { get; private set; }

    private readonly List<PurchaseOrderLine> _lines = [];
    public IReadOnlyCollection<PurchaseOrderLine> Lines => _lines.AsReadOnly();

    private PurchaseOrder() { }

    public PurchaseOrder(string poNumber, int supplierId, DateTime? expectedDate, string? notes, string? createdBy)
    {
        PoNumber = poNumber;
        SupplierId = supplierId;
        OrderDate = DateTime.UtcNow;
        ExpectedDate = expectedDate;
        Status = "DRAFT";
        Notes = notes;
        CreatedBy = createdBy;
        CreatedDate = DateTime.UtcNow;
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new PurchaseOrderCreatedEvent(poNumber, supplierId));
    }

    public void AddLine(int productId, int lineNumber, decimal quantityOrdered, decimal? unitPrice, string? notes)
    {
        if (Status != "DRAFT")
            throw new InvalidOperationException("Cannot add lines to a non-draft purchase order.");

        var line = new PurchaseOrderLine(productId, lineNumber, quantityOrdered, unitPrice, notes);
        _lines.Add(line);
    }

    public void Confirm()
    {
        if (Status != "DRAFT")
            throw new InvalidOperationException("Only draft purchase orders can be confirmed.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot confirm a purchase order with no lines.");

        Status = "CONFIRMED";
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new PurchaseOrderConfirmedEvent(PoNumber, SupplierId));
    }

    public void StartReceiving()
    {
        if (Status != "CONFIRMED" && Status != "RECEIVING")
            throw new InvalidOperationException("Purchase order must be confirmed before receiving.");

        Status = "RECEIVING";
        ModifiedDate = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status != "RECEIVING")
            throw new InvalidOperationException("Only receiving purchase orders can be completed.");

        Status = "COMPLETED";
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new PurchaseOrderCompletedEvent(PoNumber, SupplierId));
    }

    public void Cancel()
    {
        if (Status == "COMPLETED" || Status == "CANCELLED")
            throw new InvalidOperationException($"Cannot cancel a {Status} purchase order.");

        Status = "CANCELLED";
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new PurchaseOrderCancelledEvent(PoNumber, SupplierId));
    }
}
