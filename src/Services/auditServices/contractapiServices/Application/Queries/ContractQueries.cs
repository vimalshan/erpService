using ContractService.Application.DTOs;
using ContractService.Domain.Interfaces;
using MediatR;

namespace ContractService.Application.Queries;

public record GetContractByIdQuery(int ContractId) : IRequest<ContractDto?>;
public record GetAllContractsQuery() : IRequest<IEnumerable<ContractDto>>;
public record GetContractsByCompanyQuery(int CompanyId) : IRequest<IEnumerable<ContractDto>>;

public class GetContractByIdQueryHandler : IRequestHandler<GetContractByIdQuery, ContractDto?>
{
    private readonly IContractDomainRepository _repo;
    public GetContractByIdQueryHandler(IContractDomainRepository repo) { _repo = repo; }

    public async Task<ContractDto?> Handle(GetContractByIdQuery request, CancellationToken ct)
    {
        var c = await _repo.GetByIdAsync(request.ContractId);
        if (c == null) return null;
        return new ContractDto(c.ContractId, c.ContractNumber, c.ContractName, c.CompanyId, c.ContractType,
            c.StartDate, c.EndDate, c.Status, c.TotalValue, c.Currency, c.IsActive, c.CreatedDate, c.ModifiedDate,
            c.CreatedBy, c.ModifiedBy, c.SignedDate, c.SignedByClient, c.SignedByDNV, c.ContractPath, c.Terms, c.Notes,
            c.RenewalDate, c.AutoRenewal,
            c.ContractServices.Select(s => new ContractServiceDto(s.ContractServiceId, s.ContractId, s.ServiceId,
                s.Quantity, s.UnitPrice, s.TotalPrice, s.Currency, s.IsActive, s.StartDate, s.EndDate, s.Status, s.Notes)).ToList(),
            c.ContractSites.Select(s => new ContractSiteDto(s.ContractSiteId, s.ContractId, s.SiteId,
                s.IsActive, s.StartDate, s.EndDate, s.Status, s.Notes)).ToList());
    }
}

public class GetAllContractsQueryHandler : IRequestHandler<GetAllContractsQuery, IEnumerable<ContractDto>>
{
    private readonly IContractDomainRepository _repo;
    public GetAllContractsQueryHandler(IContractDomainRepository repo) { _repo = repo; }

    public async Task<IEnumerable<ContractDto>> Handle(GetAllContractsQuery request, CancellationToken ct)
    {
        var contracts = await _repo.GetAllAsync();
        return contracts.Select(c => new ContractDto(c.ContractId, c.ContractNumber, c.ContractName, c.CompanyId,
            c.ContractType, c.StartDate, c.EndDate, c.Status, c.TotalValue, c.Currency, c.IsActive,
            c.CreatedDate, c.ModifiedDate, c.CreatedBy, c.ModifiedBy, c.SignedDate, c.SignedByClient, c.SignedByDNV,
            c.ContractPath, c.Terms, c.Notes, c.RenewalDate, c.AutoRenewal,
            c.ContractServices.Select(s => new ContractServiceDto(s.ContractServiceId, s.ContractId, s.ServiceId,
                s.Quantity, s.UnitPrice, s.TotalPrice, s.Currency, s.IsActive, s.StartDate, s.EndDate, s.Status, s.Notes)).ToList(),
            c.ContractSites.Select(s => new ContractSiteDto(s.ContractSiteId, s.ContractId, s.SiteId,
                s.IsActive, s.StartDate, s.EndDate, s.Status, s.Notes)).ToList()));
    }
}

public class GetContractsByCompanyQueryHandler : IRequestHandler<GetContractsByCompanyQuery, IEnumerable<ContractDto>>
{
    private readonly IContractDomainRepository _repo;
    public GetContractsByCompanyQueryHandler(IContractDomainRepository repo) { _repo = repo; }

    public async Task<IEnumerable<ContractDto>> Handle(GetContractsByCompanyQuery request, CancellationToken ct)
    {
        var contracts = await _repo.GetByCompanyAsync(request.CompanyId);
        return contracts.Select(c => new ContractDto(c.ContractId, c.ContractNumber, c.ContractName, c.CompanyId,
            c.ContractType, c.StartDate, c.EndDate, c.Status, c.TotalValue, c.Currency, c.IsActive,
            c.CreatedDate, c.ModifiedDate, c.CreatedBy, c.ModifiedBy, c.SignedDate, c.SignedByClient, c.SignedByDNV,
            c.ContractPath, c.Terms, c.Notes, c.RenewalDate, c.AutoRenewal,
            c.ContractServices.Select(s => new ContractServiceDto(s.ContractServiceId, s.ContractId, s.ServiceId,
                s.Quantity, s.UnitPrice, s.TotalPrice, s.Currency, s.IsActive, s.StartDate, s.EndDate, s.Status, s.Notes)).ToList(),
            c.ContractSites.Select(s => new ContractSiteDto(s.ContractSiteId, s.ContractId, s.SiteId,
                s.IsActive, s.StartDate, s.EndDate, s.Status, s.Notes)).ToList()));
    }
}
