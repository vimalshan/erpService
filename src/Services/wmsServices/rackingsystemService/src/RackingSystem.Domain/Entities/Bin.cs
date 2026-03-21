using RackingSystem.Domain.Common;
using RackingSystem.Domain.Events;

namespace RackingSystem.Domain.Entities;

/// <summary>A specific storage bin (location) within a zone/shelf.</summary>
public class Bin : BaseEntity
{
    public int ZoneId { get; private set; }
    public int? ShelfId { get; private set; }
    public string Code { get; private set; } = default!;
    public string? Barcode { get; private set; }
    public string? BinType { get; private set; }
    public decimal? CapacityQty { get; private set; }
    public decimal? CapacityWeight { get; private set; }
    public decimal? CapacityVolume { get; private set; }
    public string Status { get; private set; } = "AVAILABLE";
    public bool IsActive { get; private set; } = true;

    public Shelf? Shelf { get; private set; }

    private Bin() { }

    public static Bin Create(int zoneId, string code, int? shelfId = null,
        string? barcode = null, string? binType = null,
        decimal? capacityQty = null, decimal? capacityWeight = null, decimal? capacityVolume = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code, nameof(code));

        return new Bin
        {
            ZoneId         = zoneId,
            ShelfId        = shelfId,
            Code           = code.Trim().ToUpperInvariant(),
            Barcode        = barcode,
            BinType        = binType,
            CapacityQty    = capacityQty,
            CapacityWeight = capacityWeight,
            CapacityVolume = capacityVolume,
            Status         = "AVAILABLE",
            IsActive       = true
        };
    }

    public void UpdateStatus(string newStatus)
    {
        var validStatuses = new[] { "AVAILABLE", "OCCUPIED", "BLOCKED", "FULL" };
        if (!validStatuses.Contains(newStatus.ToUpperInvariant()))
            throw new ArgumentException($"Invalid bin status: {newStatus}");

        var previousStatus = Status;
        Status       = newStatus.ToUpperInvariant();
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new BinStatusChangedEvent(Id, previousStatus, Status));
    }

    public void Update(string code, string? barcode, string? binType,
        decimal? capacityQty, decimal? capacityWeight, decimal? capacityVolume)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code, nameof(code));
        Code           = code.Trim().ToUpperInvariant();
        Barcode        = barcode;
        BinType        = binType;
        CapacityQty    = capacityQty;
        CapacityWeight = capacityWeight;
        CapacityVolume = capacityVolume;
        ModifiedDate   = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive     = false;
        ModifiedDate = DateTime.UtcNow;
    }
}
