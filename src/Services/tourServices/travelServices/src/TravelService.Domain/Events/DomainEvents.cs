using TravelService.Domain.Common;

namespace TravelService.Domain.Events;

public record TourPlanCreatedEvent(string TourPlanId, string EmployeeSysId) : DomainEvent;
public record TourPlanSubmittedEvent(string TourPlanId, string EmployeeSysId) : DomainEvent;
public record TourPlanApprovedEvent(string TourPlanId, string EmployeeSysId, string ApprovedBy) : DomainEvent;
public record TourPlanRejectedEvent(string TourPlanId, string EmployeeSysId, string RejectedBy, string Remarks) : DomainEvent;
public record TourPlanCancelledEvent(string TourPlanId, string CancelledBy) : DomainEvent;
public record TourPlanClosedEvent(string TourPlanId, string ClosedBy) : DomainEvent;
public record BatchCreatedEvent(string BatchId, string CreatedBy) : DomainEvent;
public record BatchApprovedEvent(string BatchId, string ApprovedBy) : DomainEvent;
public record ForexRequestCreatedEvent(string ForexId, string TourPlanId) : DomainEvent;
