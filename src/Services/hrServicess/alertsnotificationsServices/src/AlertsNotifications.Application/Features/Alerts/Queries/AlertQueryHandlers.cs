using AlertsNotifications.Application.DTOs;
using AutoMapper;
using AlertsNotifications.Domain.Interfaces;
using MediatR;

namespace AlertsNotifications.Application.Features.Alerts.Queries;

public class AlertQueryHandlers :
    IRequestHandler<GetAllAlertsQuery, IEnumerable<AlertMasterDto>>,
    IRequestHandler<GetAlertByIdQuery, AlertMasterDto?>,
    IRequestHandler<GetAlertsByAppQuery, IEnumerable<AlertMasterDto>>
{
    private readonly IAlertMasterRepository _repository;
    private readonly IMapper _mapper;

    public AlertQueryHandlers(IAlertMasterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AlertMasterDto>> Handle(GetAllAlertsQuery request, CancellationToken cancellationToken)
    {
        var alerts = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AlertMasterDto>>(alerts);
    }

    public async Task<AlertMasterDto?> Handle(GetAlertByIdQuery request, CancellationToken cancellationToken)
    {
        var alert = await _repository.GetByIdAsync(request.AlertId, cancellationToken);
        return alert is null ? null : _mapper.Map<AlertMasterDto>(alert);
    }

    public async Task<IEnumerable<AlertMasterDto>> Handle(GetAlertsByAppQuery request, CancellationToken cancellationToken)
    {
        var alerts = await _repository.GetByAppAsync(request.AlertApps, cancellationToken);
        return _mapper.Map<IEnumerable<AlertMasterDto>>(alerts);
    }
}
