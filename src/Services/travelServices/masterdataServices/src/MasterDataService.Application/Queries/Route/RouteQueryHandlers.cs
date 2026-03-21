using AutoMapper;
using MasterDataService.Application.DTOs;
using MasterDataService.Domain.Interfaces;
using MediatR;

namespace MasterDataService.Application.Queries.Route;

public class GetAllRoutesQueryHandler : IRequestHandler<GetAllRoutesQuery, IReadOnlyList<RouteDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllRoutesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<RouteDto>> Handle(GetAllRoutesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.Routes.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<RouteDto>>(entities);
    }
}

public class GetRouteByIdQueryHandler : IRequestHandler<GetRouteByIdQuery, RouteDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRouteByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RouteDto?> Handle(GetRouteByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Routes.GetByIdAsync(request.Id, cancellationToken);
        return entity is null ? null : _mapper.Map<RouteDto>(entity);
    }
}
