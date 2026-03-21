using WarehouseStructure.Domain.Common;
using WarehouseStructure.Domain.Events;
using WarehouseStructure.Domain.ValueObjects;

namespace WarehouseStructure.Domain.Entities;

public class Warehouse : AggregateRoot
{
    public int WarehouseId { get => Id; set => Id = value; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? AddressLine { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    private readonly List<Zone> _zones = new();
    public IReadOnlyCollection<Zone> Zones => _zones.AsReadOnly();

    public Address GetAddress() => new(AddressLine, City, State, Country, PostalCode);

    public void SetAddress(Address address)
    {
        AddressLine = address.Street;
        City = address.City;
        State = address.State;
        Country = address.Country;
        PostalCode = address.PostalCode;
    }

    public Zone AddZone(string code, string name, ZoneType zoneType, string? description = null)
    {
        var zone = new Zone
        {
            WarehouseId = WarehouseId,
            Code = code,
            Name = name,
            ZoneTypeValue = zoneType.Value,
            Description = description,
            IsActive = true
        };
        _zones.Add(zone);
        AddDomainEvent(new ZoneCreatedEvent(zone.ZoneId, WarehouseId, code, zoneType.Value));
        return zone;
    }

    public void RaiseCreatedEvent()
    {
        AddDomainEvent(new WarehouseCreatedEvent(WarehouseId, Code, Name));
    }

    public void RaiseUpdatedEvent()
    {
        AddDomainEvent(new WarehouseUpdatedEvent(WarehouseId, Code));
    }

    public void RaiseDeletedEvent()
    {
        AddDomainEvent(new WarehouseDeletedEvent(WarehouseId, Code));
    }
}
