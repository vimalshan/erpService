using MediatR;
using BusServices.Application.DTOs;
using BusServices.Domain.Interfaces;

namespace BusServices.Application.Routes.Queries;

public record GetRoutesByBusQuery(int BusId) : IRequest<IEnumerable<BusRouteDto>>;

public sealed class GetRoutesByBusQueryHandler : IRequestHandler<GetRoutesByBusQuery, IEnumerable<BusRouteDto>>
{
    private readonly IBusRouteRepository _repo;

    public GetRoutesByBusQueryHandler(IBusRouteRepository repo) => _repo = repo;

    public async Task<IEnumerable<BusRouteDto>> Handle(GetRoutesByBusQuery request, CancellationToken ct)
    {
        var routes = await _repo.GetByBusIdAsync(request.BusId, ct);
        return routes.Select(r => new BusRouteDto(
            r.RouteId, r.BusId, r.Name, r.Description,
            r.Status.Value.ToString(), r.LastModifiedBy, r.LastModifiedOn));
    }
}
