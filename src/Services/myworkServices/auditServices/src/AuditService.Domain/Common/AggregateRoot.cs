namespace AuditService.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
    // Marker class for aggregate roots supporting DDD patterns.
    // Aggregate roots control access to the aggregate's entities.
}
