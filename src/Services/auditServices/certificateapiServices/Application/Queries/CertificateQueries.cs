using CertificateService.Application.DTOs;
using CertificateService.Domain.Interfaces;
using MediatR;

namespace CertificateService.Application.Queries;

public record GetCertificateByIdQuery(int Id) : IRequest<CertificateDto?>;
public record GetAllCertificatesQuery() : IRequest<IEnumerable<CertificateDto>>;

public class GetCertificateByIdHandler : IRequestHandler<GetCertificateByIdQuery, CertificateDto?>
{
    private readonly ICertificateDomainRepository _repo;
    public GetCertificateByIdHandler(ICertificateDomainRepository repo) => _repo = repo;
    public async Task<CertificateDto?> Handle(GetCertificateByIdQuery request, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(request.Id, ct);
        return e is null ? null : new CertificateDto(e.CertificateId, e.CertificateNumber, e.CertificateName,
            e.CompanyId, e.SiteId, e.ServiceId, e.IssueDate, e.ExpiryDate, e.Status, e.CertificateType, e.Scope, e.IsActive);
    }
}

public class GetAllCertificatesHandler : IRequestHandler<GetAllCertificatesQuery, IEnumerable<CertificateDto>>
{
    private readonly ICertificateDomainRepository _repo;
    public GetAllCertificatesHandler(ICertificateDomainRepository repo) => _repo = repo;
    public async Task<IEnumerable<CertificateDto>> Handle(GetAllCertificatesQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return entities.Select(e => new CertificateDto(e.CertificateId, e.CertificateNumber, e.CertificateName,
            e.CompanyId, e.SiteId, e.ServiceId, e.IssueDate, e.ExpiryDate, e.Status, e.CertificateType, e.Scope, e.IsActive));
    }
}
