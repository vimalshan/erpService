using RackingSystem.Domain.Common;
using RackingSystem.Domain.Events;

namespace RackingSystem.Domain.Entities;

/// <summary>Physical rack structure installed within a warehouse zone.</summary>
public class Rack : BaseEntity
{
    private readonly List<Shelf> _shelves = [];

    public int ZoneId { get; private set; }
    public string Code { get; private set; } = default!;
    public string? RackType { get; private set; }
    public decimal? MaxLoadWeight { get; private set; }
    public bool IsActive { get; private set; } = true;

    public IReadOnlyCollection<Shelf> Shelves => _shelves.AsReadOnly();

    // EF Core constructor
    private Rack() { }

    public static Rack Create(int zoneId, string code, string? rackType = null, decimal? maxLoadWeight = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code, nameof(code));

        var rack = new Rack
        {
            ZoneId = zoneId,
            Code = code.Trim().ToUpperInvariant(),
            RackType = rackType?.ToUpperInvariant(),
            MaxLoadWeight = maxLoadWeight,
            IsActive = true
        };

        rack.AddDomainEvent(new RackCreatedEvent(rack));
        return rack;
    }

    public void Update(string code, string? rackType, decimal? maxLoadWeight)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code, nameof(code));
        Code = code.Trim().ToUpperInvariant();
        RackType = rackType?.ToUpperInvariant();
        MaxLoadWeight = maxLoadWeight;
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new RackUpdatedEvent(this));
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
    }

    public void AddShelf(Shelf shelf) => _shelves.Add(shelf);
}
