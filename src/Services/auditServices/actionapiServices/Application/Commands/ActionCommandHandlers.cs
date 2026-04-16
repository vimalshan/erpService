using ActionService.Application.DTOs;
using ActionService.Domain.Entities;
using ActionService.Domain.Interfaces;
using MediatR;

namespace ActionService.Application.Commands;

public class CreateActionHandler : IRequestHandler<CreateActionCommand, ActionDto>
{
    private readonly IActionRepository _repository;
    private readonly IMediator _mediator;

    public CreateActionHandler(IActionRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<ActionDto> Handle(CreateActionCommand request, CancellationToken ct)
    {
        var entity = ActionItem.Create(
            request.Dto.Action, request.Dto.DueDate, request.Dto.HighPriority,
            request.Dto.Message, request.Dto.Language, request.Dto.Service,
            request.Dto.Site, request.Dto.EntityType, request.Dto.EntityId,
            request.Dto.Subject, request.Dto.SnowLink);

        var created = await _repository.AddAsync(entity, ct);

        foreach (var domainEvent in created.DomainEvents)
            if (domainEvent is INotification notification)
                await _mediator.Publish(notification, ct);
        created.ClearDomainEvents();

        return new ActionDto(created.Id, created.Action, created.DueDate, created.HighPriority,
            created.Message, created.Language, created.Service, created.Site,
            created.EntityType, created.EntityId, created.Subject, created.SnowLink);
    }
}

public class UpdateActionHandler : IRequestHandler<UpdateActionCommand, ActionDto>
{
    private readonly IActionRepository _repository;

    public UpdateActionHandler(IActionRepository repository) => _repository = repository;

    public async Task<ActionDto> Handle(UpdateActionCommand request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Dto.Id, ct)
            ?? throw new System.Collections.Generic.KeyNotFoundException($"Action {request.Dto.Id} not found");

        entity.Action = request.Dto.Action;
        entity.DueDate = request.Dto.DueDate;
        entity.HighPriority = request.Dto.HighPriority;
        entity.Message = request.Dto.Message;
        entity.Language = request.Dto.Language;
        entity.Service = request.Dto.Service;
        entity.Site = request.Dto.Site;
        entity.EntityType = request.Dto.EntityType;
        entity.EntityId = request.Dto.EntityId;
        entity.Subject = request.Dto.Subject;
        entity.SnowLink = request.Dto.SnowLink;

        await _repository.UpdateAsync(entity, ct);
        return new ActionDto(entity.Id, entity.Action, entity.DueDate, entity.HighPriority,
            entity.Message, entity.Language, entity.Service, entity.Site,
            entity.EntityType, entity.EntityId, entity.Subject, entity.SnowLink);
    }
}

public class DeleteActionHandler : IRequestHandler<DeleteActionCommand, bool>
{
    private readonly IActionRepository _repository;
    public DeleteActionHandler(IActionRepository repository) => _repository = repository;

    public async Task<bool> Handle(DeleteActionCommand request, CancellationToken ct)
    {
        await _repository.DeleteAsync(request.Id, ct);
        return true;
    }
}

public class CompleteActionHandler : IRequestHandler<CompleteActionCommand, bool>
{
    private readonly IActionRepository _repository;
    private readonly IMediator _mediator;

    public CompleteActionHandler(IActionRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(CompleteActionCommand request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Id, ct)
            ?? throw new System.Collections.Generic.KeyNotFoundException($"Action {request.Id} not found");
        entity.MarkComplete();

        await _repository.UpdateAsync(entity, ct);

        foreach (var domainEvent in entity.DomainEvents)
            if (domainEvent is INotification notification)
                await _mediator.Publish(notification, ct);
        entity.ClearDomainEvents();

        return true;
    }
}
