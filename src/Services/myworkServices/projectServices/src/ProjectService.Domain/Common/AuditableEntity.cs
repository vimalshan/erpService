namespace ProjectService.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
}
