using ReceivingService.Domain.Common;
using ReceivingService.Domain.Events;
using ReceivingService.Domain.Exceptions;
using ReceivingService.Domain.ValueObjects;

namespace ReceivingService.Domain.Entities;

/// <summary>
/// Receiving aggregate root – models a warehouse receiving transaction
/// against a purchase order.
/// </summary>
public sealed class Receiving : AggregateRoot
{
    private readonly List<ReceivingLine> _lines = new();

    public string ReceivingNumber       { get; private set; } = null!;
    public int PoId                     { get; private set; }
    public int WarehouseId              { get; private set; }
    public DateTime ReceivedDate        { get; private set; }
    public string Status                { get; private set; } = null!;
    public string? Notes                { get; private set; }
    public string? CreatedBy            { get; private set; }
    public DateTime CreatedDate         { get; private set; }

    public IReadOnlyCollection<ReceivingLine> Lines => _lines.AsReadOnly();

    private Receiving() { }

    /// <summary>Create a new Receiving aggregate (OPEN status).</summary>
    public static Receiving Create(
        string receivingNumber,
        int poId,
        int warehouseId,
        string? notes = null,
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(receivingNumber))
            throw new ReceivingDomainException("Receiving number is required.");
        if (poId <= 0)
            throw new ReceivingDomainException("A valid Purchase Order ID is required.");
        if (warehouseId <= 0)
            throw new ReceivingDomainException("A valid Warehouse ID is required.");

        var receiving = new Receiving
        {
            ReceivingNumber = receivingNumber,
            PoId            = poId,
            WarehouseId     = warehouseId,
            ReceivedDate    = DateTime.UtcNow,
            Status          = ReceivingStatus.Open.Value,
            Notes           = notes,
            CreatedBy       = createdBy,
            CreatedDate     = DateTime.UtcNow
        };

        receiving.AddDomainEvent(new ReceivingCreatedEvent(receiving));
        return receiving;
    }

    /// <summary>Add a line item to this receiving.</summary>
    public ReceivingLine AddLine(
        int poLineId,
        int productId,
        int binId,
        decimal quantityReceived,
        string? lotNumber = null,
        DateOnly? expiryDate = null,
        string? lineNotes = null)
    {
        EnsureOpen();
        var line = ReceivingLine.Create(
            Id, poLineId, productId, binId,
            quantityReceived, lotNumber, expiryDate, lineNotes);
        _lines.Add(line);
        return line;
    }

    /// <summary>Close this receiving – no further lines can be added.</summary>
    public void Close()
    {
        EnsureOpen();
        Status = ReceivingStatus.Closed.Value;
        AddDomainEvent(new ReceivingClosedEvent(this));
    }

    /// <summary>Cancel this receiving.</summary>
    public void Cancel()
    {
        if (Status == ReceivingStatus.Cancelled.Value)
            throw new ReceivingDomainException("Receiving is already cancelled.");
        if (Status == ReceivingStatus.Closed.Value)
            throw new ReceivingDomainException("Cannot cancel a closed receiving.");
        Status = ReceivingStatus.Cancelled.Value;
        AddDomainEvent(new ReceivingCancelledEvent(this));
    }

    private void EnsureOpen()
    {
        if (Status != ReceivingStatus.Open.Value)
            throw new ReceivingDomainException($"Operation not allowed on receiving with status '{Status}'.");
    }
}
