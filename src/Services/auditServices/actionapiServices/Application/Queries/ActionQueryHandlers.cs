using ActionService.Application.DTOs;
using ActionService.Domain.Interfaces;
using MediatR;

namespace ActionService.Application.Queries;

public class GetActionByIdHandler : IRequestHandler<GetActionByIdQuery, ActionDto?>
{
    private readonly IActionRepository _repository;
    public GetActionByIdHandler(IActionRepository repository) => _repository = repository;

    public async Task<ActionDto?> Handle(GetActionByIdQuery request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Id, ct);
        if (entity is null) return null;
        return new ActionDto(entity.Id, entity.Action, entity.DueDate, entity.HighPriority,
            entity.Message, entity.Language, entity.Service, entity.Site,
            entity.EntityType, entity.EntityId, entity.Subject, entity.SnowLink);
    }
}

public class GetAllActionsHandler : IRequestHandler<GetAllActionsQuery, IEnumerable<ActionDto>>
{
    private readonly IActionRepository _repository;
    public GetAllActionsHandler(IActionRepository repository) => _repository = repository;

    public async Task<IEnumerable<ActionDto>> Handle(GetAllActionsQuery request, CancellationToken ct)
    {
        var entities = await _repository.GetAllAsync(ct);
        return entities.Select(e => new ActionDto(e.Id, e.Action, e.DueDate, e.HighPriority,
            e.Message, e.Language, e.Service, e.Site,
            e.EntityType, e.EntityId, e.Subject, e.SnowLink));
    }
}

public class GetActionsByEntityHandler : IRequestHandler<GetActionsByEntityQuery, IEnumerable<ActionDto>>
{
    private readonly IActionRepository _repository;
    public GetActionsByEntityHandler(IActionRepository repository) => _repository = repository;

    public async Task<IEnumerable<ActionDto>> Handle(GetActionsByEntityQuery request, CancellationToken ct)
    {
        var entities = await _repository.GetByEntityAsync(request.EntityType, request.EntityId, ct);
        return entities.Select(e => new ActionDto(e.Id, e.Action, e.DueDate, e.HighPriority,
            e.Message, e.Language, e.Service, e.Site,
            e.EntityType, e.EntityId, e.Subject, e.SnowLink));
    }
}
