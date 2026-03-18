using DemandManagement.Domain.Common;

namespace DemandManagement.Domain.Events;

public sealed record DemandApprovedEvent(long DemandId, long ApprovedBy) : IDomainEvent;
public sealed record DemandRejectedEvent(long DemandId, long RejectedBy) : IDomainEvent;
public sealed record DemandCompletedEvent(long DemandId, long CompletedBy) : IDomainEvent;
public sealed record DemandCreatedEvent(long DemandId, long CreatedBy) : IDomainEvent;
