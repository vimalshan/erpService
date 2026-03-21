using FleetManagement.Application.DTOs;
using MediatR;

namespace FleetManagement.Application.Commands.Routes;

public record CreateRouteCommand(
    string RouteName, string? Description,
    string? StartLocation, string? EndLocation,
    int? EstimatedDuration) : IRequest<RouteDto>;

public record UpdateRouteCommand(
    int RouteId, string RouteName, string? Description,
    string? StartLocation, string? EndLocation,
    int? EstimatedDuration) : IRequest<RouteDto>;

public record DeleteRouteCommand(int RouteId) : IRequest<bool>;
