using AuditLogService.Application.Commands;
using AuditLogService.Application.DTOs;
using MediatR;

namespace AuditLogService.API.GraphQL;

public class AuditLogMutation
{
    public async Task<AuditLogDto> CreateAuditLog(
        [Service] IMediator mediator,
        string tableName,
        int recordId,
        string action,
        string? changedBy,
        string? oldValues,
        string? newValues)
    {
        var command = new CreateAuditLogCommand(tableName, recordId, action, changedBy, oldValues, newValues);
        return await mediator.Send(command);
    }
}
