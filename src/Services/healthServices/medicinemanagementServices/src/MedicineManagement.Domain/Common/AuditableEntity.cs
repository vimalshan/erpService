namespace MedicineManagement.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public string? EntryUser { get; set; }
    public decimal? EntryUserPin { get; set; }
    public DateTime? EntryDate { get; set; }
    public string? ModifiedUser { get; set; }
    public decimal? ModifiedUserPin { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
