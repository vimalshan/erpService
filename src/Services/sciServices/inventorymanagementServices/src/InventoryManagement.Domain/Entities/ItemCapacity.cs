using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Entities;

public class ItemCapacity : AuditableEntity
{
    public int CapacityId { get; set; }
    public string CapacityName { get; set; } = default!;
}
