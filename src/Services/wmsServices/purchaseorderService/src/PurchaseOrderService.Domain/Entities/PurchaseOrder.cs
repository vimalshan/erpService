using PurchaseOrderService.Domain.Common;
using PurchaseOrderService.Domain.Enums;
using PurchaseOrderService.Domain.Events;

namespace PurchaseOrderService.Domain.Entities;

public class PurchaseOrder : AggregateRoot<int>
{
    public string PoNumber { get; private set; } = null!;
    public int SupplierId { get; private set; }
    public int WarehouseId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? ExpectedDate { get; private set; }
    public PurchaseOrderStatus Status { get; private set; }
    public string? Notes { get; private set; }
    public string? CreatedBy { get; private set; }

    private readonly List<PurchaseOrderLine> _lines = new();
    public IReadOnlyCollection<PurchaseOrderLine> Lines => _lines.AsReadOnly();

    private PurchaseOrder() { } // EF constructor

    public static PurchaseOrder Create(
        string poNumber,
        int supplierId,
        int warehouseId,
        DateTime orderDate,
        DateTime? expectedDate,
        string? notes,
        string? createdBy)
    {
        var po = new PurchaseOrder
        {
            PoNumber = poNumber ?? throw new ArgumentNullException(nameof(poNumber)),
            SupplierId = supplierId,
            WarehouseId = warehouseId,
            OrderDate = orderDate,
            ExpectedDate = expectedDate,
            Status = PurchaseOrderStatus.Draft,
            Notes = notes,
            CreatedBy = createdBy,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        po.AddDomainEvent(new PurchaseOrderCreatedEvent(po.PoNumber, po.SupplierId, po.WarehouseId));
        return po;
    }

    public PurchaseOrderLine AddLine(int productId, int lineNumber, decimal quantityOrdered, decimal? unitPrice, string? notes)
    {
        EnsureModifiable();
        if (_lines.Any(l => l.LineNumber == lineNumber))
            throw new InvalidOperationException($"Line number {lineNumber} already exists.");

        var line = new PurchaseOrderLine(Id, productId, lineNumber, quantityOrdered, unitPrice, notes);
        _lines.Add(line);
        ModifiedDate = DateTime.UtcNow;
        return line;
    }

    public void RemoveLine(int lineNumber)
    {
        EnsureModifiable();
        var line = _lines.FirstOrDefault(l => l.LineNumber == lineNumber)
            ?? throw new InvalidOperationException($"Line number {lineNumber} not found.");
        _lines.Remove(line);
        ModifiedDate = DateTime.UtcNow;
    }

    public void Confirm()
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Only DRAFT orders can be confirmed.");
        if (!_lines.Any())
            throw new InvalidOperationException("Cannot confirm an order with no lines.");

        Status = PurchaseOrderStatus.Confirmed;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new PurchaseOrderConfirmedEvent(PoNumber));
    }

    public void StartReceiving()
    {
        if (Status != PurchaseOrderStatus.Confirmed)
            throw new InvalidOperationException("Only CONFIRMED orders can start receiving.");

        Status = PurchaseOrderStatus.Receiving;
        ModifiedDate = DateTime.UtcNow;
    }

    public void ReceiveLine(int lineNumber, decimal quantity)
    {
        if (Status != PurchaseOrderStatus.Confirmed && Status != PurchaseOrderStatus.Receiving)
            throw new InvalidOperationException("Order must be CONFIRMED or RECEIVING to receive items.");

        var line = _lines.FirstOrDefault(l => l.LineNumber == lineNumber)
            ?? throw new InvalidOperationException($"Line number {lineNumber} not found.");

        line.Receive(quantity);

        if (Status == PurchaseOrderStatus.Confirmed)
        {
            Status = PurchaseOrderStatus.Receiving;
        }

        if (_lines.All(l => l.IsFullyReceived))
        {
            Status = PurchaseOrderStatus.Completed;
            AddDomainEvent(new PurchaseOrderCompletedEvent(PoNumber));
        }

        ModifiedDate = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == PurchaseOrderStatus.Completed)
            throw new InvalidOperationException("Completed orders cannot be cancelled.");
        if (Status == PurchaseOrderStatus.Cancelled)
            throw new InvalidOperationException("Order is already cancelled.");

        Status = PurchaseOrderStatus.Cancelled;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new PurchaseOrderCancelledEvent(PoNumber));
    }

    public void Update(DateTime? expectedDate, string? notes)
    {
        EnsureModifiable();
        ExpectedDate = expectedDate;
        Notes = notes;
        ModifiedDate = DateTime.UtcNow;
    }

    public decimal? TotalAmount => _lines.Where(l => l.LineTotal.HasValue).Sum(l => l.LineTotal);

    private void EnsureModifiable()
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidOperationException("Only DRAFT orders can be modified.");
    }
}
