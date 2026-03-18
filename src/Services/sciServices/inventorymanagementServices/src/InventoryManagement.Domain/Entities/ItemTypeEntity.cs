using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Entities;

public class ItemType : AuditableEntity
{
    public int ItemTypeId { get; set; }
    public string? ItemTypeCode { get; set; }
    public string? Description { get; set; }
}
