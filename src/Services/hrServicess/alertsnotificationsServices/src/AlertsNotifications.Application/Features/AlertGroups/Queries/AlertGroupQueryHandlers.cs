using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AlertsNotifications.Application.Features.AlertGroups.Queries;

public class AlertGroupQueryHandlers :
    IRequestHandler<GetAllAlertGroupsQuery, IEnumerable<AlertGroupDto>>,
    IRequestHandler<GetAlertGroupByIdQuery, AlertGroupDto?>
{
    private readonly IAlertGroupRepository _repository;
    private readonly IMapper _mapper;

    public AlertGroupQueryHandlers(IAlertGroupRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AlertGroupDto>> Handle(GetAllAlertGroupsQuery request, CancellationToken cancellationToken)
    {
        var groups = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<AlertGroupDto>>(groups);
    }

    public async Task<AlertGroupDto?> Handle(GetAlertGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var group = await _repository.GetByIdAsync(request.AlertGroupId, cancellationToken);
        return group is null ? null : _mapper.Map<AlertGroupDto>(group);
    }
}
