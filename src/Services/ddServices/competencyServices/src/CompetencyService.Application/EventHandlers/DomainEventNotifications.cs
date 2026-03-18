using MediatR;
using CompetencyService.Domain.Events;

namespace CompetencyService.Application.EventHandlers;

/// <summary>MediatR notifications wrapping domain events (adapter pattern).</summary>
public record CompetencyCreatedDomainEventNotification(CompetencyCreatedEvent Event) : INotification;
public record CompetencyUpdatedDomainEventNotification(CompetencyUpdatedEvent Event) : INotification;
public record CompetencyClosedDomainEventNotification(CompetencyClosedEvent Event) : INotification;
public record EmpCompetencyAssignedDomainEventNotification(EmpCompetencyAssignedEvent Event) : INotification;
