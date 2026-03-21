using AuditLogService.Application.DTOs;
using AuditLogService.Application.Queries;
using MediatR;

namespace AuditLogService.API.GraphQL;

public class AuditLogQuery
{
    public async Task<IReadOnlyList<AuditLogDto>> GetAuditLogs([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllAuditLogsQuery());
    }

    public async Task<AuditLogDto?> GetAuditLogById([Service] IMediator mediator, long id)
    {
        return await mediator.Send(new GetAuditLogByIdQuery(id));
    }

    public async Task<IReadOnlyList<AuditLogDto>> GetAuditLogsByTable([Service] IMediator mediator, string tableName)
    {
        return await mediator.Send(new GetAuditLogsByTableQuery(tableName));
    }
}
