using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Entities;

public class MaterialTaxClass : AuditableEntity
{
    public int MaterialTaxClassId { get; set; }
    public string? Description { get; set; }
}
