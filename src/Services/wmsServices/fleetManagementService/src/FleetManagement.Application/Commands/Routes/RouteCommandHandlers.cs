using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Interfaces;
using MediatR;

namespace FleetManagement.Application.Commands.Routes;

public class CreateRouteHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CreateRouteCommand, RouteDto>
{
    public async Task<RouteDto> Handle(CreateRouteCommand request, CancellationToken ct)
    {
        var route = new Route
        {
            RouteName = request.RouteName,
            Description = request.Description,
            StartLocation = request.StartLocation,
            EndLocation = request.EndLocation,
            EstimatedDuration = request.EstimatedDuration
        };
        await uow.Routes.AddAsync(route, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<RouteDto>(route);
    }
}

public class UpdateRouteHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpdateRouteCommand, RouteDto>
{
    public async Task<RouteDto> Handle(UpdateRouteCommand request, CancellationToken ct)
    {
        var route = await uow.Routes.GetByIdAsync(request.RouteId, ct)
            ?? throw new KeyNotFoundException($"Route {request.RouteId} not found.");
        route.RouteName = request.RouteName;
        route.Description = request.Description;
        route.StartLocation = request.StartLocation;
        route.EndLocation = request.EndLocation;
        route.EstimatedDuration = request.EstimatedDuration;
        await uow.Routes.UpdateAsync(route, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<RouteDto>(route);
    }
}

public class DeleteRouteHandler(IUnitOfWork uow) : IRequestHandler<DeleteRouteCommand, bool>
{
    public async Task<bool> Handle(DeleteRouteCommand request, CancellationToken ct)
    {
        var route = await uow.Routes.GetByIdAsync(request.RouteId, ct);
        if (route is null) return false;
        await uow.Routes.DeleteAsync(route, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
