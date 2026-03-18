using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AlertsNotifications.Application.Features.Circulars.Queries;

public class CircularQueryHandlers :
    IRequestHandler<GetAllCircularsQuery, IEnumerable<CircularDto>>,
    IRequestHandler<GetCircularByIdQuery, CircularDto?>,
    IRequestHandler<GetCircularsByStatusQuery, IEnumerable<CircularDto>>,
    IRequestHandler<GetCircularsByOrgIdQuery, IEnumerable<CircularDto>>
{
    private readonly ICircularRepository _repository;
    private readonly IMapper _mapper;

    public CircularQueryHandlers(ICircularRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CircularDto>> Handle(GetAllCircularsQuery request, CancellationToken cancellationToken)
    {
        var circulars = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CircularDto>>(circulars);
    }

    public async Task<CircularDto?> Handle(GetCircularByIdQuery request, CancellationToken cancellationToken)
    {
        var circular = await _repository.GetByIdAsync(request.CircularId, cancellationToken);
        return circular is null ? null : _mapper.Map<CircularDto>(circular);
    }

    public async Task<IEnumerable<CircularDto>> Handle(GetCircularsByStatusQuery request, CancellationToken cancellationToken)
    {
        var circulars = await _repository.GetByStatusAsync(request.Status, cancellationToken);
        return _mapper.Map<IEnumerable<CircularDto>>(circulars);
    }

    public async Task<IEnumerable<CircularDto>> Handle(GetCircularsByOrgIdQuery request, CancellationToken cancellationToken)
    {
        var circulars = await _repository.GetByOrgIdAsync(request.OrgId, cancellationToken);
        return _mapper.Map<IEnumerable<CircularDto>>(circulars);
    }
}
