namespace ReceivingService.Domain.Common;

public abstract class AggregateRoot : Entity
{
    // Inherits domain event management from Entity.
    // Marking it as a separate concept allows infrastructure to detect
    // aggregate boundaries for transaction scoping.
}
