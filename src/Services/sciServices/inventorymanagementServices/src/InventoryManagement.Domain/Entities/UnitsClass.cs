namespace InventoryManagement.Domain.Entities;

public class UnitsClass
{
    public int UnitsClassId { get; set; }
    public string? UnitsClassName { get; set; }

    public ICollection<UnitOfMeasure> Units { get; set; } = new List<UnitOfMeasure>();
}
