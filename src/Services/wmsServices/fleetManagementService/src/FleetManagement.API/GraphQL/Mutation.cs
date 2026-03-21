using FleetManagement.Application.Commands.Drivers;
using FleetManagement.Application.Commands.Maintenance;
using FleetManagement.Application.Commands.Routes;
using FleetManagement.Application.Commands.Trips;
using FleetManagement.Application.Commands.Vehicles;
using FleetManagement.Application.DTOs;
using MediatR;

namespace FleetManagement.API.GraphQL;

public class Mutation
{
    public async Task<VehicleDto> CreateVehicle([Service] IMediator mediator, CreateVehicleCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<VehicleDto> UpdateVehicle([Service] IMediator mediator, UpdateVehicleCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<VehicleDto> ChangeVehicleStatus([Service] IMediator mediator, ChangeVehicleStatusCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<DriverDto> CreateDriver([Service] IMediator mediator, CreateDriverCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<DriverDto> UpdateDriver([Service] IMediator mediator, UpdateDriverCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<RouteDto> CreateRoute([Service] IMediator mediator, CreateRouteCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<TripDto> CreateTrip([Service] IMediator mediator, CreateTripCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<TripDto> StartTrip([Service] IMediator mediator, StartTripCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<TripDto> CompleteTrip([Service] IMediator mediator, CompleteTripCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<TripDto> CancelTrip([Service] IMediator mediator, CancelTripCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<MaintenanceLogDto> LogMaintenance([Service] IMediator mediator, LogMaintenanceCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<FuelLogDto> LogFuel([Service] IMediator mediator, LogFuelCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);
}
