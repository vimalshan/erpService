namespace LovService.Domain.Entities;

public class ItemData
{
    public int Id { get; private set; }
    public string? CatName { get; private set; }
    public string? ItemName { get; private set; }
    public string? Make { get; private set; }
    public string? Uom { get; private set; }
    public int? Price { get; private set; }

    // EF Core requires a parameterless constructor
    private ItemData() { }

    public static ItemData Create(string? catName, string? itemName, string? make, string? uom, int? price)
    {
        if (catName?.Length > 40) throw new ArgumentException("CatName cannot exceed 40 characters.");
        if (itemName?.Length > 60) throw new ArgumentException("ItemName cannot exceed 60 characters.");
        if (make?.Length > 30) throw new ArgumentException("Make cannot exceed 30 characters.");
        if (uom?.Length > 20) throw new ArgumentException("Uom cannot exceed 20 characters.");

        return new ItemData { CatName = catName, ItemName = itemName, Make = make, Uom = uom, Price = price };
    }

    public void Update(string? catName, string? itemName, string? make, string? uom, int? price)
    {
        CatName = catName;
        ItemName = itemName;
        Make = make;
        Uom = uom;
        Price = price;
    }
}
