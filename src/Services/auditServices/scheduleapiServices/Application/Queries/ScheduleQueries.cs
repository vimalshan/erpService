using ScheduleService.Application.DTOs;
using ScheduleService.Domain.Interfaces;
using MediatR;

namespace ScheduleService.Application.Queries;

public record GetScheduleByIdQuery(int AuditSiteAuditId) : IRequest<AuditSiteAuditDto?>;
public record GetAllSchedulesQuery() : IRequest<IEnumerable<AuditSiteAuditDto>>;
public record GetSchedulesByAuditQuery(int AuditId) : IRequest<IEnumerable<AuditSiteAuditDto>>;
public record GetSchedulesBySiteQuery(int SiteId) : IRequest<IEnumerable<AuditSiteAuditDto>>;

public class GetScheduleByIdQueryHandler : IRequestHandler<GetScheduleByIdQuery, AuditSiteAuditDto?>
{
    private readonly IScheduleDomainRepository _repo;
    public GetScheduleByIdQueryHandler(IScheduleDomainRepository repo) { _repo = repo; }

    public async Task<AuditSiteAuditDto?> Handle(GetScheduleByIdQuery request, CancellationToken ct)
    {
        var a = await _repo.GetByIdAsync(request.AuditSiteAuditId);
        if (a == null) return null;
        return new AuditSiteAuditDto(a.AuditSiteAuditId, a.AuditId, a.SiteId, a.AuditTypeId, a.AuditNumber,
            a.ScheduledDate, a.StartDate, a.EndDate, a.CompletedDate, a.Status, a.LeadAuditorId,
            a.IsActive, a.CreatedDate, a.ModifiedDate, a.CreatedBy, a.ModifiedBy, a.Notes,
            a.ReportPath, a.CertificateIssued, a.CertificateNumber);
    }
}

public class GetAllSchedulesQueryHandler : IRequestHandler<GetAllSchedulesQuery, IEnumerable<AuditSiteAuditDto>>
{
    private readonly IScheduleDomainRepository _repo;
    public GetAllSchedulesQueryHandler(IScheduleDomainRepository repo) { _repo = repo; }

    public async Task<IEnumerable<AuditSiteAuditDto>> Handle(GetAllSchedulesQuery request, CancellationToken ct)
    {
        var list = await _repo.GetAllAsync();
        return list.Select(a => new AuditSiteAuditDto(a.AuditSiteAuditId, a.AuditId, a.SiteId, a.AuditTypeId,
            a.AuditNumber, a.ScheduledDate, a.StartDate, a.EndDate, a.CompletedDate, a.Status,
            a.LeadAuditorId, a.IsActive, a.CreatedDate, a.ModifiedDate, a.CreatedBy, a.ModifiedBy,
            a.Notes, a.ReportPath, a.CertificateIssued, a.CertificateNumber));
    }
}

public class GetSchedulesByAuditQueryHandler : IRequestHandler<GetSchedulesByAuditQuery, IEnumerable<AuditSiteAuditDto>>
{
    private readonly IScheduleDomainRepository _repo;
    public GetSchedulesByAuditQueryHandler(IScheduleDomainRepository repo) { _repo = repo; }

    public async Task<IEnumerable<AuditSiteAuditDto>> Handle(GetSchedulesByAuditQuery request, CancellationToken ct)
    {
        var list = await _repo.GetByAuditAsync(request.AuditId);
        return list.Select(a => new AuditSiteAuditDto(a.AuditSiteAuditId, a.AuditId, a.SiteId, a.AuditTypeId,
            a.AuditNumber, a.ScheduledDate, a.StartDate, a.EndDate, a.CompletedDate, a.Status,
            a.LeadAuditorId, a.IsActive, a.CreatedDate, a.ModifiedDate, a.CreatedBy, a.ModifiedBy,
            a.Notes, a.ReportPath, a.CertificateIssued, a.CertificateNumber));
    }
}

public class GetSchedulesBySiteQueryHandler : IRequestHandler<GetSchedulesBySiteQuery, IEnumerable<AuditSiteAuditDto>>
{
    private readonly IScheduleDomainRepository _repo;
    public GetSchedulesBySiteQueryHandler(IScheduleDomainRepository repo) { _repo = repo; }

    public async Task<IEnumerable<AuditSiteAuditDto>> Handle(GetSchedulesBySiteQuery request, CancellationToken ct)
    {
        var list = await _repo.GetBySiteAsync(request.SiteId);
        return list.Select(a => new AuditSiteAuditDto(a.AuditSiteAuditId, a.AuditId, a.SiteId, a.AuditTypeId,
            a.AuditNumber, a.ScheduledDate, a.StartDate, a.EndDate, a.CompletedDate, a.Status,
            a.LeadAuditorId, a.IsActive, a.CreatedDate, a.ModifiedDate, a.CreatedBy, a.ModifiedBy,
            a.Notes, a.ReportPath, a.CertificateIssued, a.CertificateNumber));
    }
}
