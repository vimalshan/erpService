using ScheduleService.Application.DTOs;
using MediatR;

namespace ScheduleService.Application.Commands;

public record ScheduleAuditCommand(CreateAuditSiteAuditDto Dto) : IRequest<AuditSiteAuditDto>;
public record UpdateScheduleCommand(UpdateAuditSiteAuditDto Dto) : IRequest<AuditSiteAuditDto>;
public record DeleteScheduleCommand(int AuditSiteAuditId) : IRequest<bool>;
public record RescheduleAuditCommand(int AuditSiteAuditId, DateTime? NewDate, int? ModifiedBy) : IRequest<AuditSiteAuditDto>;
public record StartAuditCommand(int AuditSiteAuditId, DateTime StartDate, int? ModifiedBy) : IRequest<AuditSiteAuditDto>;
public record CompleteAuditCommand(int AuditSiteAuditId, DateTime CompletedDate, string? ReportPath, int? ModifiedBy) : IRequest<AuditSiteAuditDto>;
