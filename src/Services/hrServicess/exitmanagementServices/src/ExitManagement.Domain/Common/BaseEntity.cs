namespace ExitManagement.Domain.Common;

public abstract class BaseEntity
{
    public DateTime CreatedOn { get; protected set; } = DateTime.UtcNow;
    public decimal? UpdatedBy { get; protected set; }
    public DateTime? UpdatedOn { get; protected set; }
}
