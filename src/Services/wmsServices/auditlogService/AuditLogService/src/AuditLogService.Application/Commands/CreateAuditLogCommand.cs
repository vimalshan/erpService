using AuditLogService.Application.DTOs;
using MediatR;

namespace AuditLogService.Application.Commands;

public record CreateAuditLogCommand(
    string TableName,
    int RecordId,
    string Action,
    string? ChangedBy,
    string? OldValues,
    string? NewValues) : IRequest<AuditLogDto>;
