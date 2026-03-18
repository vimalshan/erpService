using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Entities;

public class PackageType : AuditableEntity
{
    public int PackageTypeId { get; set; }
    public string PackageTypeName { get; set; } = default!;

    public ICollection<ItemMaster> Items { get; set; } = new List<ItemMaster>();
}
