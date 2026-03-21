using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.Application.Commands.Route;

public record CreateRouteCommand(int RouteId, string RouteName) : IRequest<RouteDto>;
public record UpdateRouteCommand(long Id, string RouteName) : IRequest<RouteDto>;
public record DeleteRouteCommand(long Id) : IRequest<bool>;
