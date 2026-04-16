using ContractService.Application.DTOs;
using ContractService.Domain.Entities;
using ContractService.Domain.Interfaces;
using MediatR;

namespace ContractService.Application.Commands;

public class CreateContractCommandHandler : IRequestHandler<CreateContractCommand, ContractDto>
{
    private readonly IContractDomainRepository _repo;
    private readonly IMediator _mediator;
    public CreateContractCommandHandler(IContractDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<ContractDto> Handle(CreateContractCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var entity = Contract.Create(d.ContractNumber, d.ContractName, d.CompanyId, d.ContractType,
            d.StartDate, d.EndDate, d.TotalValue, d.Currency, d.CreatedBy);
        entity.SignedByClient = d.SignedByClient; entity.SignedByDNV = d.SignedByDNV;
        entity.ContractPath = d.ContractPath; entity.Terms = d.Terms; entity.Notes = d.Notes;
        entity.AutoRenewal = d.AutoRenewal;

        var created = await _repo.AddAsync(entity);
        foreach (var evt in created.DomainEvents) await _mediator.Publish(evt, ct);
        created.ClearDomainEvents();
        return MapToDto(created);
    }

    private static ContractDto MapToDto(Contract c) => new(
        c.ContractId, c.ContractNumber, c.ContractName, c.CompanyId, c.ContractType,
        c.StartDate, c.EndDate, c.Status, c.TotalValue, c.Currency, c.IsActive,
        c.CreatedDate, c.ModifiedDate, c.CreatedBy, c.ModifiedBy, c.SignedDate,
        c.SignedByClient, c.SignedByDNV, c.ContractPath, c.Terms, c.Notes,
        c.RenewalDate, c.AutoRenewal,
        c.ContractServices.Select(s => new ContractServiceDto(s.ContractServiceId, s.ContractId, s.ServiceId,
            s.Quantity, s.UnitPrice, s.TotalPrice, s.Currency, s.IsActive, s.StartDate, s.EndDate, s.Status, s.Notes)).ToList(),
        c.ContractSites.Select(s => new ContractSiteDto(s.ContractSiteId, s.ContractId, s.SiteId,
            s.IsActive, s.StartDate, s.EndDate, s.Status, s.Notes)).ToList());
}

public class UpdateContractCommandHandler : IRequestHandler<UpdateContractCommand, ContractDto>
{
    private readonly IContractDomainRepository _repo;
    private readonly IMediator _mediator;
    public UpdateContractCommandHandler(IContractDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<ContractDto> Handle(UpdateContractCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var entity = await _repo.GetByIdAsync(d.ContractId) ?? throw new System.Collections.Generic.KeyNotFoundException($"Contract {d.ContractId} not found");
        entity.ContractNumber = d.ContractNumber; entity.ContractName = d.ContractName; entity.CompanyId = d.CompanyId;
        entity.ContractType = d.ContractType; entity.StartDate = d.StartDate; entity.EndDate = d.EndDate;
        entity.Status = d.Status; entity.TotalValue = d.TotalValue; entity.Currency = d.Currency;
        entity.IsActive = d.IsActive; entity.SignedByClient = d.SignedByClient; entity.SignedByDNV = d.SignedByDNV;
        entity.ContractPath = d.ContractPath; entity.Terms = d.Terms; entity.Notes = d.Notes;
        entity.RenewalDate = d.RenewalDate; entity.AutoRenewal = d.AutoRenewal;
        entity.ModifiedDate = DateTime.UtcNow; entity.ModifiedBy = d.ModifiedBy;

        await _repo.UpdateAsync(entity);
        foreach (var evt in entity.DomainEvents) await _mediator.Publish(evt, ct);
        entity.ClearDomainEvents();
        return new ContractDto(entity.ContractId, entity.ContractNumber, entity.ContractName, entity.CompanyId,
            entity.ContractType, entity.StartDate, entity.EndDate, entity.Status, entity.TotalValue, entity.Currency,
            entity.IsActive, entity.CreatedDate, entity.ModifiedDate, entity.CreatedBy, entity.ModifiedBy,
            entity.SignedDate, entity.SignedByClient, entity.SignedByDNV, entity.ContractPath, entity.Terms, entity.Notes,
            entity.RenewalDate, entity.AutoRenewal,
            entity.ContractServices.Select(s => new ContractServiceDto(s.ContractServiceId, s.ContractId, s.ServiceId,
                s.Quantity, s.UnitPrice, s.TotalPrice, s.Currency, s.IsActive, s.StartDate, s.EndDate, s.Status, s.Notes)).ToList(),
            entity.ContractSites.Select(s => new ContractSiteDto(s.ContractSiteId, s.ContractId, s.SiteId,
                s.IsActive, s.StartDate, s.EndDate, s.Status, s.Notes)).ToList());
    }
}

public class DeleteContractCommandHandler : IRequestHandler<DeleteContractCommand, bool>
{
    private readonly IContractDomainRepository _repo;
    public DeleteContractCommandHandler(IContractDomainRepository repo) { _repo = repo; }

    public async Task<bool> Handle(DeleteContractCommand request, CancellationToken ct)
    {
        await _repo.DeleteAsync(request.ContractId);
        return true;
    }
}

public class ChangeContractStatusCommandHandler : IRequestHandler<ChangeContractStatusCommand, ContractDto>
{
    private readonly IContractDomainRepository _repo;
    private readonly IMediator _mediator;
    public ChangeContractStatusCommandHandler(IContractDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<ContractDto> Handle(ChangeContractStatusCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.ContractId) ?? throw new System.Collections.Generic.KeyNotFoundException($"Contract {request.ContractId} not found");
        entity.ChangeStatus(request.NewStatus, request.ModifiedBy);
        await _repo.UpdateAsync(entity);
        foreach (var evt in entity.DomainEvents) await _mediator.Publish(evt, ct);
        entity.ClearDomainEvents();
        return new ContractDto(entity.ContractId, entity.ContractNumber, entity.ContractName, entity.CompanyId,
            entity.ContractType, entity.StartDate, entity.EndDate, entity.Status, entity.TotalValue, entity.Currency,
            entity.IsActive, entity.CreatedDate, entity.ModifiedDate, entity.CreatedBy, entity.ModifiedBy,
            entity.SignedDate, entity.SignedByClient, entity.SignedByDNV, entity.ContractPath, entity.Terms, entity.Notes,
            entity.RenewalDate, entity.AutoRenewal, new List<ContractServiceDto>(), new List<ContractSiteDto>());
    }
}

public class RenewContractCommandHandler : IRequestHandler<RenewContractCommand, ContractDto>
{
    private readonly IContractDomainRepository _repo;
    private readonly IMediator _mediator;
    public RenewContractCommandHandler(IContractDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<ContractDto> Handle(RenewContractCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.ContractId) ?? throw new System.Collections.Generic.KeyNotFoundException($"Contract {request.ContractId} not found");
        entity.Renew(request.NewEndDate, request.ModifiedBy);
        await _repo.UpdateAsync(entity);
        foreach (var evt in entity.DomainEvents) await _mediator.Publish(evt, ct);
        entity.ClearDomainEvents();
        return new ContractDto(entity.ContractId, entity.ContractNumber, entity.ContractName, entity.CompanyId,
            entity.ContractType, entity.StartDate, entity.EndDate, entity.Status, entity.TotalValue, entity.Currency,
            entity.IsActive, entity.CreatedDate, entity.ModifiedDate, entity.CreatedBy, entity.ModifiedBy,
            entity.SignedDate, entity.SignedByClient, entity.SignedByDNV, entity.ContractPath, entity.Terms, entity.Notes,
            entity.RenewalDate, entity.AutoRenewal, new List<ContractServiceDto>(), new List<ContractSiteDto>());
    }
}
