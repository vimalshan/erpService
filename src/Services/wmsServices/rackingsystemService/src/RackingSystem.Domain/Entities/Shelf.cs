using RackingSystem.Domain.Common;

namespace RackingSystem.Domain.Entities;

/// <summary>A shelf (level/position) within a rack.</summary>
public class Shelf : BaseEntity
{
    public int RackId { get; private set; }
    public int ShelfLevel { get; private set; }
    public int ShelfPosition { get; private set; }
    public string Code { get; private set; } = default!;
    public decimal? CapacityQty { get; private set; }
    public decimal? CapacityWeight { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Rack? Rack { get; private set; }

    private Shelf() { }

    public static Shelf Create(int rackId, int shelfLevel, int shelfPosition, string code,
        decimal? capacityQty = null, decimal? capacityWeight = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code, nameof(code));

        return new Shelf
        {
            RackId        = rackId,
            ShelfLevel    = shelfLevel,
            ShelfPosition = shelfPosition,
            Code          = code.Trim().ToUpperInvariant(),
            CapacityQty   = capacityQty,
            CapacityWeight = capacityWeight,
            IsActive = true
        };
    }

    public void Update(string code, decimal? capacityQty, decimal? capacityWeight)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code, nameof(code));
        Code           = code.Trim().ToUpperInvariant();
        CapacityQty    = capacityQty;
        CapacityWeight = capacityWeight;
        ModifiedDate   = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive     = false;
        ModifiedDate = DateTime.UtcNow;
    }
}
