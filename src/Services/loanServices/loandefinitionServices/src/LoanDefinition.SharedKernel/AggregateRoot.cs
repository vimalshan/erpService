namespace LoanDefinition.SharedKernel;

public abstract class AggregateRoot<TId> : BaseEntity<TId> where TId : notnull
{
    public long CreatedBy { get; protected set; }
    public DateTime CreatedOn { get; protected set; }
    public long LastModifiedBy { get; protected set; }
    public DateTime LastModifiedOn { get; protected set; }
}
