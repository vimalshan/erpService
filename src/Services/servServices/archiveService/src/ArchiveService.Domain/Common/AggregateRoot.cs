namespace ArchiveService.Domain.Common;

public abstract class AggregateRoot<TId> : BaseEntity where TId : notnull
{
    public TId Id { get; protected set; } = default!;
}
