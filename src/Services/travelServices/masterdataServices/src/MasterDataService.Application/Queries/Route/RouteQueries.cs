using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.Application.Queries.Route;

public record GetAllRoutesQuery : IRequest<IReadOnlyList<RouteDto>>;
public record GetRouteByIdQuery(long Id) : IRequest<RouteDto?>;
