namespace EmployeeService.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public long LastModifiedBy { get; protected set; }
    public DateTime LastModifiedOn { get; protected set; }
}
