using FleetManagement.Application.DTOs;
using FleetManagement.Application.Queries.Drivers;
using FleetManagement.Application.Queries.FleetStatus;
using FleetManagement.Application.Queries.Maintenance;
using FleetManagement.Application.Queries.Routes;
using FleetManagement.Application.Queries.Trips;
using FleetManagement.Application.Queries.Vehicles;
using MediatR;

namespace FleetManagement.API.GraphQL;

public class Query
{
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<VehicleDto>> GetVehicles([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllVehiclesQuery(), ct);

    public async Task<VehicleDto?> GetVehicle([Service] IMediator mediator, int vehicleId, CancellationToken ct)
        => await mediator.Send(new GetVehicleByIdQuery(vehicleId), ct);

    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<DriverDto>> GetDrivers([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllDriversQuery(), ct);

    public async Task<DriverDto?> GetDriver([Service] IMediator mediator, int driverId, CancellationToken ct)
        => await mediator.Send(new GetDriverByIdQuery(driverId), ct);

    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<TripDto>> GetTrips([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllTripsQuery(), ct);

    public async Task<TripDto?> GetTrip([Service] IMediator mediator, int tripId, CancellationToken ct)
        => await mediator.Send(new GetTripWithStopsQuery(tripId), ct);

    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<RouteDto>> GetRoutes([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllRoutesQuery(), ct);

    public async Task<IReadOnlyList<MaintenanceLogDto>> GetMaintenanceLogs([Service] IMediator mediator, int vehicleId, CancellationToken ct)
        => await mediator.Send(new GetMaintenanceByVehicleQuery(vehicleId), ct);

    public async Task<IReadOnlyList<FuelLogDto>> GetFuelLogs([Service] IMediator mediator, int vehicleId, CancellationToken ct)
        => await mediator.Send(new GetFuelLogsByVehicleQuery(vehicleId), ct);

    public async Task<IEnumerable<FleetStatusDto>> GetFleetStatus([Service] IMediator mediator, int? warehouseId, CancellationToken ct)
        => await mediator.Send(new GetFleetStatusQuery(warehouseId), ct);
}
