using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Domain.Entities;
using AlertsNotifications.Domain.Events;
using AlertsNotifications.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AlertsNotifications.Application.Features.Alerts.Commands;

public class AlertCommandHandlers :
    IRequestHandler<CreateAlertCommand, AlertMasterDto>,
    IRequestHandler<UpdateAlertCommand, Unit>,
    IRequestHandler<DeleteAlertCommand, Unit>
{
    private readonly IAlertMasterRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public AlertCommandHandlers(IAlertMasterRepository repository, IMapper mapper, IMediator mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AlertMasterDto> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
    {
        var entity = new AlertMaster
        {
            AlertId = request.AlertId,
            AlertApps = request.AlertApps,
            AlertName = request.AlertName,
            AlertType = request.AlertType,
            AlertDesc = request.AlertDesc,
            AlertToDesc = request.AlertToDesc,
            AlertCcDesc = request.AlertCcDesc,
            AlertGradeCat = request.AlertGradeCat,
            AlertUnitSpecific = request.AlertUnitSpecific
        };

        var created = await _repository.AddAsync(entity, cancellationToken);
        await _mediator.Publish(new AlertCreatedEvent(created.AlertId, created.AlertName), cancellationToken);
        return _mapper.Map<AlertMasterDto>(created);
    }

    public async Task<Unit> Handle(UpdateAlertCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.AlertId, cancellationToken)
            ?? throw new KeyNotFoundException($"Alert with ID {request.AlertId} not found.");

        entity.AlertApps = request.AlertApps;
        entity.AlertName = request.AlertName;
        entity.AlertType = request.AlertType;
        entity.AlertDesc = request.AlertDesc;
        entity.AlertToDesc = request.AlertToDesc;
        entity.AlertCcDesc = request.AlertCcDesc;
        entity.AlertGradeCat = request.AlertGradeCat;
        entity.AlertUnitSpecific = request.AlertUnitSpecific;

        await _repository.UpdateAsync(entity, cancellationToken);
        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteAlertCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.AlertId, cancellationToken);
        return Unit.Value;
    }
}
