using AuditService.Application.DTOs;
using MediatR;

namespace AuditService.Application.Queries;

public record GetAuditByIdQuery(int Id) : IRequest<AuditDto?>;
public record GetAllAuditsQuery() : IRequest<IEnumerable<AuditDto>>;
public record GetAuditTypesQuery() : IRequest<IEnumerable<AuditTypeDto>>;
public record GetSiteAuditsQuery(int AuditId) : IRequest<IEnumerable<AuditSiteAuditDto>>;
