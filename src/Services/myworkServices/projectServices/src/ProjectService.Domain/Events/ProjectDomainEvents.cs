using ProjectService.Domain.Common;

namespace ProjectService.Domain.Events;

public sealed record ProjectCreatedEvent(long ProjectId, string ProjectName) : DomainEvent;

public sealed record ProjectStatusChangedEvent(long ProjectId, char OldStatus, char NewStatus) : DomainEvent;

public sealed record ProjectApprovalRequestedEvent(long ProjectId, long ApprovalId, char ApprovalType) : DomainEvent;

public sealed record ProjectApprovedEvent(long ProjectId, long ApprovalId) : DomainEvent;

public sealed record ProjectRejectedEvent(long ProjectId, long ApprovalId, string Remarks) : DomainEvent;

public sealed record ProjectHeldEvent(long ProjectId, string Reason) : DomainEvent;

public sealed record ProjectUnheldEvent(long ProjectId, string Reason) : DomainEvent;

public sealed record ProjectClosedEvent(long ProjectId, DateTime ClosedDate) : DomainEvent;

public sealed record ProjectMemberAddedEvent(long ProjectId, long MemberId, long EmployeeId) : DomainEvent;

public sealed record ProjectMemberRemovedEvent(long ProjectId, long MemberId) : DomainEvent;
