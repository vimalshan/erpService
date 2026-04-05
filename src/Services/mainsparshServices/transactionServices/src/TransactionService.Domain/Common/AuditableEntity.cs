namespace TransactionService.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public long CreatedBy { get; protected set; }
    public DateTime CreatedOn { get; protected set; }
    public long? UpdatedBy { get; protected set; }
    public DateTime? UpdatedOn { get; protected set; }
}
