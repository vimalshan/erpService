using AuditLogService.Application.DTOs;
using MediatR;

namespace AuditLogService.Application.Queries;

public record GetAuditLogByIdQuery(long Id) : IRequest<AuditLogDto?>;
