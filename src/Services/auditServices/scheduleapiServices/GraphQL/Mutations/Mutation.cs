using ScheduleService.Application.Commands;
using ScheduleService.Application.DTOs;
using MediatR;

namespace ScheduleService.GraphQL.Mutations;

public class Mutation
{
    public async Task<AuditSiteAuditDto> ScheduleAudit([Service] IMediator mediator, CreateAuditSiteAuditDto input)
        => await mediator.Send(new ScheduleAuditCommand(input));

    public async Task<AuditSiteAuditDto> UpdateSchedule([Service] IMediator mediator, UpdateAuditSiteAuditDto input)
        => await mediator.Send(new UpdateScheduleCommand(input));

    public async Task<bool> DeleteSchedule([Service] IMediator mediator, int auditSiteAuditId)
        => await mediator.Send(new DeleteScheduleCommand(auditSiteAuditId));

    public async Task<AuditSiteAuditDto> RescheduleAudit([Service] IMediator mediator, int auditSiteAuditId, DateTime? newDate, int? modifiedBy)
        => await mediator.Send(new RescheduleAuditCommand(auditSiteAuditId, newDate, modifiedBy));

    public async Task<AuditSiteAuditDto> StartAudit([Service] IMediator mediator, int auditSiteAuditId, DateTime startDate, int? modifiedBy)
        => await mediator.Send(new StartAuditCommand(auditSiteAuditId, startDate, modifiedBy));

    public async Task<AuditSiteAuditDto> CompleteAudit([Service] IMediator mediator, int auditSiteAuditId, DateTime completedDate, string? reportPath, int? modifiedBy)
        => await mediator.Send(new CompleteAuditCommand(auditSiteAuditId, completedDate, reportPath, modifiedBy));
}
