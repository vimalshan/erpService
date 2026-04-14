using WMTransactional.Domain.Common;
using WMTransactional.Domain.Events;

namespace WMTransactional.Domain.Entities;

public class SalesOrder : BaseEntity
{
    public int SoId { get; private set; }
    public string SoNumber { get; private set; } = null!;
    public int CustomerId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? RequestedDate { get; private set; }
    public string Status { get; private set; } = null!;
    public string? Notes { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime ModifiedDate { get; private set; }

    private readonly List<SalesOrderLine> _lines = [];
    public IReadOnlyCollection<SalesOrderLine> Lines => _lines.AsReadOnly();

    private SalesOrder() { }

    public SalesOrder(string soNumber, int customerId, DateTime? requestedDate, string? notes, string? createdBy)
    {
        SoNumber = soNumber;
        CustomerId = customerId;
        OrderDate = DateTime.UtcNow;
        RequestedDate = requestedDate;
        Status = "DRAFT";
        Notes = notes;
        CreatedBy = createdBy;
        CreatedDate = DateTime.UtcNow;
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new SalesOrderCreatedEvent(soNumber, customerId));
    }

    public void AddLine(int productId, int lineNumber, decimal quantityOrdered, decimal? unitPrice, string? notes)
    {
        if (Status != "DRAFT")
            throw new InvalidOperationException("Cannot add lines to a non-draft sales order.");

        var line = new SalesOrderLine(productId, lineNumber, quantityOrdered, unitPrice, notes);
        _lines.Add(line);
    }

    public void Confirm()
    {
        if (Status != "DRAFT")
            throw new InvalidOperationException("Only draft sales orders can be confirmed.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot confirm a sales order with no lines.");

        Status = "CONFIRMED";
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new SalesOrderConfirmedEvent(SoNumber, CustomerId));
    }

    public void StartPicking()
    {
        if (Status != "CONFIRMED" && Status != "PICKING")
            throw new InvalidOperationException("Sales order must be confirmed before picking.");

        Status = "PICKING";
        ModifiedDate = DateTime.UtcNow;
    }

    public void StartShipping()
    {
        if (Status != "PICKING" && Status != "SHIPPING")
            throw new InvalidOperationException("Sales order must be in picking status before shipping.");

        Status = "SHIPPING";
        ModifiedDate = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status != "SHIPPING")
            throw new InvalidOperationException("Only shipping sales orders can be completed.");

        Status = "COMPLETED";
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new SalesOrderCompletedEvent(SoNumber, CustomerId));
    }

    public void Cancel()
    {
        if (Status == "COMPLETED" || Status == "CANCELLED")
            throw new InvalidOperationException($"Cannot cancel a {Status} sales order.");

        Status = "CANCELLED";
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new SalesOrderCancelledEvent(SoNumber, CustomerId));
    }
}
