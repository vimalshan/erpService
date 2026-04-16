using CertificateService.Application.DTOs;
using CertificateService.Domain.Entities;
using CertificateService.Domain.Events;
using CertificateService.Domain.Interfaces;
using MediatR;

namespace CertificateService.Application.Commands;

public class CreateCertificateHandler : IRequestHandler<CreateCertificateCommand, CertificateDto>
{
    private readonly ICertificateDomainRepository _repo;
    private readonly IMediator _mediator;
    public CreateCertificateHandler(ICertificateDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<CertificateDto> Handle(CreateCertificateCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var entity = new Certificate
        {
            CertificateNumber = d.CertificateNumber, CertificateName = d.CertificateName,
            CompanyId = d.CompanyId, SiteId = d.SiteId, ServiceId = d.ServiceId,
            IssueDate = d.IssueDate, ExpiryDate = d.ExpiryDate,
            CertificateType = d.CertificateType, Scope = d.Scope
        };
        entity.AddDomainEvent(new CertificateIssuedEvent(0));
        var created = await _repo.AddAsync(entity, ct);
        foreach (var evt in created.DomainEvents) if (evt is INotification n) await _mediator.Publish(n, ct);
        created.ClearDomainEvents();
        return new CertificateDto(created.CertificateId, created.CertificateNumber, created.CertificateName,
            created.CompanyId, created.SiteId, created.ServiceId, created.IssueDate, created.ExpiryDate,
            created.Status, created.CertificateType, created.Scope, created.IsActive);
    }
}

public class UpdateCertificateHandler : IRequestHandler<UpdateCertificateCommand, CertificateDto>
{
    private readonly ICertificateDomainRepository _repo;
    public UpdateCertificateHandler(ICertificateDomainRepository repo) => _repo = repo;
    public async Task<CertificateDto> Handle(UpdateCertificateCommand request, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(request.Dto.CertificateId, ct) ?? throw new System.Collections.Generic.KeyNotFoundException();
        var d = request.Dto;
        e.CertificateNumber = d.CertificateNumber; e.CertificateName = d.CertificateName;
        e.CompanyId = d.CompanyId; e.SiteId = d.SiteId; e.ServiceId = d.ServiceId;
        e.IssueDate = d.IssueDate; e.ExpiryDate = d.ExpiryDate;
        e.Status = d.Status; e.CertificateType = d.CertificateType; e.Scope = d.Scope;
        await _repo.UpdateAsync(e, ct);
        return new CertificateDto(e.CertificateId, e.CertificateNumber, e.CertificateName,
            e.CompanyId, e.SiteId, e.ServiceId, e.IssueDate, e.ExpiryDate,
            e.Status, e.CertificateType, e.Scope, e.IsActive);
    }
}

public class DeleteCertificateHandler : IRequestHandler<DeleteCertificateCommand, bool>
{
    private readonly ICertificateDomainRepository _repo;
    public DeleteCertificateHandler(ICertificateDomainRepository repo) => _repo = repo;
    public async Task<bool> Handle(DeleteCertificateCommand request, CancellationToken ct) { await _repo.DeleteAsync(request.Id, ct); return true; }
}
