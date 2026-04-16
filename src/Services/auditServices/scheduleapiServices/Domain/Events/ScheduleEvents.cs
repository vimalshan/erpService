using MediatR;

namespace ScheduleService.Domain.Events;

public interface IDomainEvent : INotification { }

public record AuditScheduledEvent(int AuditSiteAuditId, string AuditNumber, int AuditId, int SiteId, DateTime? ScheduledDate) : IDomainEvent;
public record AuditRescheduledEvent(int AuditSiteAuditId, string AuditNumber, DateTime? OldDate, DateTime? NewDate) : IDomainEvent;
public record AuditStartedEvent(int AuditSiteAuditId, string AuditNumber, DateTime StartDate) : IDomainEvent;
public record AuditCompletedEvent(int AuditSiteAuditId, string AuditNumber, DateTime CompletedDate) : IDomainEvent;
