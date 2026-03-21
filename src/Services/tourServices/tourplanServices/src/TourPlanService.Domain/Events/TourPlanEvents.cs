using TourPlanService.Domain.Common;

namespace TourPlanService.Domain.Events;

public sealed record TourPlanCreatedEvent(
    Guid Id,
    string TourPlanId,
    string EmployeeId,
    string CreatedBy,
    DateTime OccurredOn) : IDomainEvent;

public sealed record TourPlanApprovedEvent(
    Guid Id,
    string TourPlanId,
    string ApprovedBy,
    DateTime OccurredOn) : IDomainEvent;

public sealed record TourPlanStatusChangedEvent(
    Guid Id,
    string TourPlanId,
    string OldStatus,
    string NewStatus,
    string ChangedBy,
    DateTime OccurredOn) : IDomainEvent;

public sealed record TourPlanExpenseSubmittedEvent(
    Guid Id,
    string TourPlanId,
    string SubmittedBy,
    DateTime OccurredOn) : IDomainEvent;

public sealed record ForexRequisitionCreatedEvent(
    Guid Id,
    string ForexRequisitionId,
    string TourPlanId,
    string CreatedBy,
    DateTime OccurredOn) : IDomainEvent;
