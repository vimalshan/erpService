namespace CompetencyService.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public decimal? ModifiedBy { get; protected set; }
    public DateTime? ModifiedOn { get; protected set; }

    protected void SetAudit(decimal? modifiedBy)
    {
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
