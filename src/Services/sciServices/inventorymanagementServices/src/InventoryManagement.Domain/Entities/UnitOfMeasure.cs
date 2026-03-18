using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Entities;

public class UnitOfMeasure : AuditableEntity
{
    public int UnitId { get; set; }
    public string UnitCode { get; set; } = default!;
    public string UnitOfMeasurement { get; set; } = default!;
    public int UnitClassId { get; set; }
    public char BaseUnitFlag { get; set; }
    public string? Description { get; set; }

    public UnitsClass? UnitsClass { get; set; }
}
