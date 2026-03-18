using TrustService.Domain.Common;

namespace TrustService.Domain.Events;

public sealed record TrustCreatedEvent(string TrustCode, string TrustName) : DomainEvent;
public sealed record TrustUpdatedEvent(string TrustCode, string TrustName) : DomainEvent;
public sealed record TrustClosedEvent(string TrustCode) : DomainEvent;
public sealed record TrustStatusChangedEvent(string TrustCode, string NewStatus) : DomainEvent;
public sealed record TrustFundTypeAddedEvent(string TrustCode, string FundType, string FundName) : DomainEvent;
public sealed record TrustRoleAssignedEvent(string TrustCode, string UserId, string RoleCode) : DomainEvent;
public sealed record TrustUnitAddedEvent(string TrustCode, string UnitCode, string UnitName) : DomainEvent;
public sealed record TrustApproverAddedEvent(string TrustCode, long ApproverSysId, int Level) : DomainEvent;
public sealed record TrustConfigurationChangedEvent(string TrustCode, string ConfigName, string ConfigValue) : DomainEvent;
