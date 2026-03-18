using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Entities;

public class MainProductMaster : AuditableEntity
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public int? UnitId { get; set; }
    public int? ProductTypeId { get; set; }
    public int? CompanyUnitId { get; set; }
    public char? MamFlag { get; set; }

    // Navigation
    public ProductTypeMaster? ProductType { get; set; }
    public UnitOfMeasure? Unit { get; set; }
    public ICollection<ItemMaster> Items { get; set; } = new List<ItemMaster>();
}
