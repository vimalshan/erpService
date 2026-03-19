namespace ProductionManagement.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public int? SciUserIdCreated { get; set; }
    public DateTime? CreationDate { get; set; }
    public int? SciUserIdModified { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
