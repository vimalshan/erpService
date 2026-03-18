using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Entities;

public class ItemGrade : AuditableEntity
{
    public int ItemGradeId { get; set; }
    public string ItemGradeName { get; set; } = default!;
}
