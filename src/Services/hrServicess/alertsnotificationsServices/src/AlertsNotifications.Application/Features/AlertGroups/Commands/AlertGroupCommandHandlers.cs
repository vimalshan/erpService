using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Domain.Entities;
using AlertsNotifications.Domain.Events;
using AlertsNotifications.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AlertsNotifications.Application.Features.AlertGroups.Commands;

public class AlertGroupCommandHandlers :
    IRequestHandler<CreateAlertGroupCommand, AlertGroupDto>,
    IRequestHandler<UpdateAlertGroupCommand, Unit>,
    IRequestHandler<DeleteAlertGroupCommand, Unit>
{
    private readonly IAlertGroupRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public AlertGroupCommandHandlers(IAlertGroupRepository repository, IMapper mapper, IMediator mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AlertGroupDto> Handle(CreateAlertGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = new AlertGroup
        {
            AlertGroupId = request.AlertGroupId,
            AlertGroupName = request.AlertGroupName,
            AlertGroupType = request.AlertGroupType,
            CreatedBy = request.CreatedBy,
            CreatedOn = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(entity, cancellationToken);
        await _mediator.Publish(new AlertGroupCreatedEvent(created.AlertGroupId, created.AlertGroupName), cancellationToken);
        return _mapper.Map<AlertGroupDto>(created);
    }

    public async Task<Unit> Handle(UpdateAlertGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.AlertGroupId, cancellationToken)
            ?? throw new KeyNotFoundException($"Alert group with ID {request.AlertGroupId} not found.");

        entity.AlertGroupName = request.AlertGroupName;
        entity.AlertGroupType = request.AlertGroupType;
        entity.ModifiedBy = request.ModifiedBy;
        entity.ModifiedOn = DateTime.UtcNow;

        await _repository.UpdateAsync(entity, cancellationToken);
        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteAlertGroupCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.AlertGroupId, cancellationToken);
        return Unit.Value;
    }
}
