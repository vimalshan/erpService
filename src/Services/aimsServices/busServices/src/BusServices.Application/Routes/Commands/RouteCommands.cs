using MediatR;
using BusServices.Application.DTOs;
using BusServices.Domain.Interfaces;

namespace BusServices.Application.Routes.Commands;

// ─── Create Route ─────────────────────────────────────────────────────────────

public record CreateRouteCommand(
    int BusId,
    string Name,
    string? Description,
    long CreatedBy) : IRequest<BusRouteDto>;

public sealed class CreateRouteCommandHandler : IRequestHandler<CreateRouteCommand, BusRouteDto>
{
    private readonly IBusRepository _busRepo;
    private readonly IBusRouteRepository _routeRepo;

    public CreateRouteCommandHandler(IBusRepository busRepo, IBusRouteRepository routeRepo)
    {
        _busRepo = busRepo;
        _routeRepo = routeRepo;
    }

    public async Task<BusRouteDto> Handle(CreateRouteCommand request, CancellationToken ct)
    {
        var bus = await _busRepo.GetByIdAsync(request.BusId, ct)
            ?? throw new KeyNotFoundException($"Bus {request.BusId} not found.");

        int nextId = await _routeRepo.GetNextIdAsync(ct);
        var route = bus.AddRoute(nextId, request.Name, request.Description, request.CreatedBy);

        await _routeRepo.AddAsync(route, ct);
        await _routeRepo.SaveChangesAsync(ct);

        return new BusRouteDto(route.RouteId, route.BusId, route.Name,
            route.Description, route.Status.Value.ToString(),
            route.LastModifiedBy, route.LastModifiedOn);
    }
}
