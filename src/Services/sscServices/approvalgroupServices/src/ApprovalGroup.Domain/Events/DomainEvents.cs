using ApprovalGroup.Domain.Entities;

namespace ApprovalGroup.Domain.Events;

public sealed record ApprovalGroupCreatedEvent(long GroupId, string GroupName) : IDomainEvent;

public sealed record ApprovalGroupUpdatedEvent(long GroupId, string GroupName) : IDomainEvent;

public sealed record ApprovalGroupDeletedEvent(long GroupId) : IDomainEvent;

public sealed record UserMappedToGroupEvent(long GroupId, long UserId) : IDomainEvent;

public sealed record UserRemovedFromGroupEvent(long GroupId, long UserId) : IDomainEvent;

public sealed record PullMatrixCreatedEvent(long MatId, long UnitId) : IDomainEvent;
