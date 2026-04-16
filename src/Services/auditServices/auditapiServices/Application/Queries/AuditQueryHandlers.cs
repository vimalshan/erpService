using AuditService.Application.DTOs;
using AuditService.Domain.Interfaces;
using MediatR;

namespace AuditService.Application.Queries;

public class GetAuditByIdHandler : IRequestHandler<GetAuditByIdQuery, AuditDto?>
{
    private readonly IAuditDomainRepository _repo;
    public GetAuditByIdHandler(IAuditDomainRepository repo) => _repo = repo;
    public async Task<AuditDto?> Handle(GetAuditByIdQuery request, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(request.Id, ct);
        return e is null ? null : new AuditDto(e.AuditId, e.Sites, e.Services, e.CompanyId, e.Status, e.StartDate, e.EndDate, e.LeadAuditor, e.Type);
    }
}

public class GetAllAuditsHandler : IRequestHandler<GetAllAuditsQuery, IEnumerable<AuditDto>>
{
    private readonly IAuditDomainRepository _repo;
    public GetAllAuditsHandler(IAuditDomainRepository repo) => _repo = repo;
    public async Task<IEnumerable<AuditDto>> Handle(GetAllAuditsQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return entities.Select(e => new AuditDto(e.AuditId, e.Sites, e.Services, e.CompanyId, e.Status, e.StartDate, e.EndDate, e.LeadAuditor, e.Type));
    }
}

public class GetAuditTypesHandler : IRequestHandler<GetAuditTypesQuery, IEnumerable<AuditTypeDto>>
{
    private readonly IAuditDomainRepository _repo;
    public GetAuditTypesHandler(IAuditDomainRepository repo) => _repo = repo;
    public async Task<IEnumerable<AuditTypeDto>> Handle(GetAuditTypesQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetAuditTypesAsync(ct);
        return entities.Select(e => new AuditTypeDto(e.AuditTypeId, e.AuditTypeName, e.AuditTypeCode, e.Description, e.Duration, e.IsActive, e.Category));
    }
}

public class GetSiteAuditsHandler : IRequestHandler<GetSiteAuditsQuery, IEnumerable<AuditSiteAuditDto>>
{
    private readonly IAuditDomainRepository _repo;
    public GetSiteAuditsHandler(IAuditDomainRepository repo) => _repo = repo;
    public async Task<IEnumerable<AuditSiteAuditDto>> Handle(GetSiteAuditsQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetSiteAuditsAsync(request.AuditId, ct);
        return entities.Select(e => new AuditSiteAuditDto(e.AuditSiteAuditId, e.AuditId, e.SiteId, e.AuditTypeId, e.AuditNumber, e.ScheduledDate, e.StartDate, e.EndDate, e.Status, e.LeadAuditorId));
    }
}
