namespace TrainingDevelopment.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public decimal? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}
