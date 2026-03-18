namespace VendorService.Domain.Common;

public abstract class AggregateRoot : Entity
{
    // Marker base class for aggregate roots; allows distinguishing
    // aggregate entries from child entities in the domain model.
}
