namespace AuditLogService.Application.DTOs;

public record CreateAuditLogDto(
    string TableName,
    int RecordId,
    string Action,
    string? ChangedBy,
    string? OldValues,
    string? NewValues);
