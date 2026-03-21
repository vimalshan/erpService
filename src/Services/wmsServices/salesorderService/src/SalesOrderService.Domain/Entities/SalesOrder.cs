using SalesOrderService.Domain.Common;
using SalesOrderService.Domain.Enums;
using SalesOrderService.Domain.Events;
using SalesOrderService.Domain.Exceptions;
using SalesOrderService.Domain.ValueObjects;

namespace SalesOrderService.Domain.Entities;

/// <summary>
/// SalesOrder aggregate root — owns the collection of SalesOrderLine items.
/// </summary>
public sealed class SalesOrder : AggregateRoot
{
    private readonly List<SalesOrderLine> _lines = [];

    // EF Core constructor
    private SalesOrder() { }

    public string SoNumber { get; private set; } = default!;
    public int CustomerId { get; private set; }
    public int WarehouseId { get; private set; }
    public DateOnly OrderDate { get; private set; }
    public DateOnly? RequestedDate { get; private set; }
    public SalesOrderStatus Status { get; private set; }
    public Money? TotalAmount { get; private set; }
    public string? Notes { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime ModifiedDate { get; private set; }

    public IReadOnlyCollection<SalesOrderLine> Lines => _lines.AsReadOnly();

    // ── Factory ─────────────────────────────────────────────────────────────
    public static SalesOrder Create(
        string soNumber,
        int customerId,
        int warehouseId,
        DateOnly orderDate,
        DateOnly? requestedDate,
        string? notes,
        string? createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(soNumber);
        if (customerId <= 0)  throw new SalesOrderDomainException("CustomerId must be positive.");
        if (warehouseId <= 0) throw new SalesOrderDomainException("WarehouseId must be positive.");

        var order = new SalesOrder
        {
            SoNumber      = soNumber,
            CustomerId    = customerId,
            WarehouseId   = warehouseId,
            OrderDate     = orderDate,
            RequestedDate = requestedDate,
            Status        = SalesOrderStatus.Draft,
            Notes         = notes,
            CreatedBy     = createdBy,
            CreatedDate   = DateTime.UtcNow,
            ModifiedDate  = DateTime.UtcNow
        };

        order.RaiseDomainEvent(new SalesOrderCreatedEvent(order.SoNumber, order.CustomerId));
        return order;
    }

    // ── Line Management ──────────────────────────────────────────────────────
    public SalesOrderLine AddLine(int productId, int lineNumber, decimal quantityOrdered,
        decimal? unitPrice, decimal discount = 0, string? notes = null)
    {
        if (_lines.Any(l => l.LineNumber == lineNumber))
            throw new SalesOrderDomainException($"Line number {lineNumber} already exists.");

        var line = SalesOrderLine.Create(Id, productId, lineNumber, quantityOrdered, unitPrice, discount, notes);
        _lines.Add(line);
        RecalculateTotal();
        Touch();
        return line;
    }

    public void RemoveLine(int lineNumber)
    {
        var line = _lines.SingleOrDefault(l => l.LineNumber == lineNumber)
            ?? throw new SalesOrderDomainException($"Line {lineNumber} not found.");
        _lines.Remove(line);
        RecalculateTotal();
        Touch();
    }

    // ── Status Transitions ───────────────────────────────────────────────────
    public void Confirm()
    {
        if (Status != SalesOrderStatus.Draft)
            throw new SalesOrderDomainException("Only DRAFT orders can be confirmed.");
        if (!_lines.Any())
            throw new SalesOrderDomainException("Cannot confirm an order with no lines.");

        Status = SalesOrderStatus.Confirmed;
        Touch();
        RaiseDomainEvent(new SalesOrderConfirmedEvent(SoNumber, CustomerId));
    }

    public void StartPicking()   => Transition(SalesOrderStatus.Confirmed, SalesOrderStatus.Picking);
    public void StartShipping()  => Transition(SalesOrderStatus.Picking,   SalesOrderStatus.Shipping);
    public void Complete()
    {
        Transition(SalesOrderStatus.Shipping, SalesOrderStatus.Completed);
        RaiseDomainEvent(new SalesOrderCompletedEvent(SoNumber, CustomerId));
    }

    public void Cancel(string reason)
    {
        if (Status == SalesOrderStatus.Completed)
            throw new SalesOrderDomainException("Completed orders cannot be cancelled.");
        Status = SalesOrderStatus.Cancelled;
        Notes  = string.IsNullOrWhiteSpace(reason) ? Notes : $"Cancelled: {reason}";
        Touch();
        RaiseDomainEvent(new SalesOrderCancelledEvent(SoNumber, CustomerId, reason));
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
        Touch();
    }

    // ── Private Helpers ──────────────────────────────────────────────────────
    private void Transition(SalesOrderStatus from, SalesOrderStatus to)
    {
        if (Status != from)
            throw new SalesOrderDomainException($"Cannot move to {to} from {Status}.");
        Status = to;
        Touch();
    }

    private void RecalculateTotal()
    {
        var total = _lines.Sum(l => l.LineTotal);
        TotalAmount = total > 0 ? new Money(total, "USD") : null;
    }

    private void Touch() => ModifiedDate = DateTime.UtcNow;
}
