using AuditService.Application.DTOs;
using AuditService.Domain.Entities;
using AuditService.Domain.Interfaces;
using MediatR;

namespace AuditService.Application.Commands;

public class CreateAuditHandler : IRequestHandler<CreateAuditCommand, AuditDto>
{
    private readonly IAuditDomainRepository _repository;
    private readonly IMediator _mediator;

    public CreateAuditHandler(IAuditDomainRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<AuditDto> Handle(CreateAuditCommand request, CancellationToken ct)
    {
        var entity = new Audit
        {
            Sites = request.Dto.Sites,
            Services = request.Dto.Services,
            CompanyId = request.Dto.CompanyId,
            Status = request.Dto.Status,
            StartDate = request.Dto.StartDate,
            EndDate = request.Dto.EndDate,
            LeadAuditor = request.Dto.LeadAuditor,
            Type = request.Dto.Type
        };
        entity.AddDomainEvent(new Domain.Events.AuditCreatedEvent(0));

        var created = await _repository.AddAsync(entity, ct);

        foreach (var evt in created.DomainEvents)
            if (evt is INotification notification)
                await _mediator.Publish(notification, ct);
        created.ClearDomainEvents();

        return new AuditDto(created.AuditId, created.Sites, created.Services, created.CompanyId,
            created.Status, created.StartDate, created.EndDate, created.LeadAuditor, created.Type);
    }
}

public class UpdateAuditHandler : IRequestHandler<UpdateAuditCommand, AuditDto>
{
    private readonly IAuditDomainRepository _repository;
    public UpdateAuditHandler(IAuditDomainRepository repository) => _repository = repository;

    public async Task<AuditDto> Handle(UpdateAuditCommand request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Dto.AuditId, ct)
            ?? throw new System.Collections.Generic.KeyNotFoundException($"Audit {request.Dto.AuditId} not found");
        entity.Sites = request.Dto.Sites;
        entity.Services = request.Dto.Services;
        entity.CompanyId = request.Dto.CompanyId;
        entity.Status = request.Dto.Status;
        entity.StartDate = request.Dto.StartDate;
        entity.EndDate = request.Dto.EndDate;
        entity.LeadAuditor = request.Dto.LeadAuditor;
        entity.Type = request.Dto.Type;
        await _repository.UpdateAsync(entity, ct);
        return new AuditDto(entity.AuditId, entity.Sites, entity.Services, entity.CompanyId,
            entity.Status, entity.StartDate, entity.EndDate, entity.LeadAuditor, entity.Type);
    }
}

public class DeleteAuditHandler : IRequestHandler<DeleteAuditCommand, bool>
{
    private readonly IAuditDomainRepository _repository;
    public DeleteAuditHandler(IAuditDomainRepository repository) => _repository = repository;
    public async Task<bool> Handle(DeleteAuditCommand request, CancellationToken ct)
    {
        await _repository.DeleteAsync(request.Id, ct);
        return true;
    }
}

public class ChangeAuditStatusHandler : IRequestHandler<ChangeAuditStatusCommand, bool>
{
    private readonly IAuditDomainRepository _repository;
    private readonly IMediator _mediator;

    public ChangeAuditStatusHandler(IAuditDomainRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(ChangeAuditStatusCommand request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.AuditId, ct)
            ?? throw new System.Collections.Generic.KeyNotFoundException($"Audit {request.AuditId} not found");
        entity.ChangeStatus(request.NewStatus);
        await _repository.UpdateAsync(entity, ct);

        foreach (var evt in entity.DomainEvents)
            if (evt is INotification notification)
                await _mediator.Publish(notification, ct);
        entity.ClearDomainEvents();
        return true;
    }
}
