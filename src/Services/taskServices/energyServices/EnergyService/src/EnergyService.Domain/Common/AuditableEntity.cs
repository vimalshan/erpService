namespace EnergyService.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public int LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
}
