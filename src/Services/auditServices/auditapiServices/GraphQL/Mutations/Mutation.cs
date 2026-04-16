using AuditService.Application.Commands;
using AuditService.Application.DTOs;
using AuditService.Application.Queries;
using MediatR;

namespace AuditService.GraphQL.Mutations;

public class Mutation
{
    public async Task<AuditDto> CreateAudit([Service] IMediator mediator, CreateAuditDto input)
        => await mediator.Send(new CreateAuditCommand(input));

    public async Task<AuditDto> UpdateAudit([Service] IMediator mediator, UpdateAuditDto input)
        => await mediator.Send(new UpdateAuditCommand(input));

    public async Task<bool> DeleteAudit([Service] IMediator mediator, int id)
        => await mediator.Send(new DeleteAuditCommand(id));

    public async Task<bool> ChangeAuditStatus([Service] IMediator mediator, int auditId, string newStatus)
        => await mediator.Send(new ChangeAuditStatusCommand(auditId, newStatus));
}
