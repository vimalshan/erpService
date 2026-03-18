using LocationServices.Domain.Common;

namespace LocationServices.Domain.Events;

public sealed record LocationAppMapCreatedEvent(
    decimal LocationId,
    string AppName,
    string CreatedBy) : DomainEvent;

public sealed record LocationAppMapUpdatedEvent(
    decimal LocationId,
    string AppName,
    string ModifiedBy) : DomainEvent;

public sealed record LocationAppMapDeletedEvent(
    decimal LocationId,
    string AppName,
    string ModifiedBy) : DomainEvent;
