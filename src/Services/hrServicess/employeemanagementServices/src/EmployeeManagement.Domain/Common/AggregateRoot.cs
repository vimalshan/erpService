namespace EmployeeManagement.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
    public long Id { get; protected set; }
    public DateTime CreatedOn { get; protected set; } = DateTime.UtcNow;
    public long CreatedBy { get; protected set; }
    public DateTime? UpdatedOn { get; protected set; }
    public long? UpdatedBy { get; protected set; }

    protected void SetAudit(long createdBy)
    {
        CreatedBy = createdBy;
        CreatedOn = DateTime.UtcNow;
    }

    protected void UpdateAudit(long updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
