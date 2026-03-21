using AuditLogService.Application.DTOs;
using MediatR;

namespace AuditLogService.Application.Queries;

public record GetAllAuditLogsQuery : IRequest<IReadOnlyList<AuditLogDto>>;
