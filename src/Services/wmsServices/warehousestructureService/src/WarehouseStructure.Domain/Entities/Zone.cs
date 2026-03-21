using WarehouseStructure.Domain.Common;
using WarehouseStructure.Domain.ValueObjects;

namespace WarehouseStructure.Domain.Entities;

public class Zone : BaseEntity
{
    public int ZoneId { get => Id; set => Id = value; }
    public int WarehouseId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ZoneTypeValue { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public Warehouse Warehouse { get; set; } = null!;

    public ZoneType GetZoneType() => new(ZoneTypeValue);

    public void SetZoneType(ZoneType zoneType)
    {
        ZoneTypeValue = zoneType.Value;
    }
}
