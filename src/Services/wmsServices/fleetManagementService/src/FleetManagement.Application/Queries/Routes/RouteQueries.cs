using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Interfaces;
using MediatR;

namespace FleetManagement.Application.Queries.Routes;

public record GetRouteByIdQuery(int RouteId) : IRequest<RouteDto?>;
public record GetAllRoutesQuery : IRequest<IReadOnlyList<RouteDto>>;

public class GetRouteByIdHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetRouteByIdQuery, RouteDto?>
{
    public async Task<RouteDto?> Handle(GetRouteByIdQuery request, CancellationToken ct)
    {
        var route = await uow.Routes.GetByIdAsync(request.RouteId, ct);
        return route is null ? null : mapper.Map<RouteDto>(route);
    }
}

public class GetAllRoutesHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetAllRoutesQuery, IReadOnlyList<RouteDto>>
{
    public async Task<IReadOnlyList<RouteDto>> Handle(GetAllRoutesQuery request, CancellationToken ct)
    {
        var routes = await uow.Routes.GetAllAsync(ct);
        return routes.Select(mapper.Map<RouteDto>).ToList();
    }
}
