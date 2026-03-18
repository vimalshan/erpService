using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Entities;

public class ProductTypeMaster : AuditableEntity
{
    public int ProductTypeId { get; set; }
    public string TypeName { get; set; } = default!;
    public string? TypeDescription { get; set; }

    public ICollection<MainProductMaster> Products { get; set; } = new List<MainProductMaster>();
}
