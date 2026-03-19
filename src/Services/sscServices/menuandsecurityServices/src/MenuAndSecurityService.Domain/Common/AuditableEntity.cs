namespace MenuAndSecurityService.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public long ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
}
