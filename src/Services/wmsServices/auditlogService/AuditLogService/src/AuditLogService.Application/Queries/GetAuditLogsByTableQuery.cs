using AuditLogService.Application.DTOs;
using MediatR;

namespace AuditLogService.Application.Queries;

public record GetAuditLogsByTableQuery(string TableName) : IRequest<IReadOnlyList<AuditLogDto>>;
