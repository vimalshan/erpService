using AuditService.Application.DTOs;
using MediatR;

namespace AuditService.Application.Queries.Audits;

public record GetAuditByIdQuery(long AuditId) : IRequest<AuditDto?>;

public record GetAllAuditsQuery(int PageNumber = 1, int PageSize = 20) : IRequest<IEnumerable<AuditDto>>;

public record GetAuditsByUnitQuery(long UnitId) : IRequest<IEnumerable<AuditDto>>;
