namespace InventoryManagement.Domain.Common;

public abstract class AuditableEntity
{
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? ModifiedBy { get; set; }
    public string? ModifiedDate { get; set; }
}
