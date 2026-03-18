using BusServices.Application.Buses.Commands;
using BusServices.Application.Routes.Commands;
using BusServices.Application.EmployeeAssignments.Commands;
using BusServices.Application.Arrivals.Commands;
using BusServices.Application.DTOs;
using MediatR;

namespace BusServices.API.GraphQL;

public sealed class BusMutation
{
    public async Task<BusDto> RegisterBus(
        string registrationNumber, string? description, int capacity, long registeredBy,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new RegisterBusCommand(registrationNumber, description, capacity, registeredBy), ct);

    public async Task<BusRouteDto> CreateRoute(
        int busId, string name, string? description, long createdBy,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateRouteCommand(busId, name, description, createdBy), ct);

    public async Task<EmployeeBusDto> AssignEmployeeToBus(
        long empSysId, int busId, int routeId, long assignedBy,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new AssignEmployeeToBusCommand(empSysId, busId, routeId, assignedBy), ct);

    public async Task<BusArrivalDto> RecordArrival(
        int busId, DateTime arrivalDate, string arrivalTime, string status, string? remarks, long recordedBy,
        [Service] IMediator mediator, CancellationToken ct)
    {
        var time = TimeOnly.Parse(arrivalTime);
        return await mediator.Send(new RecordArrivalCommand(busId, arrivalDate, time, status[0], remarks, recordedBy), ct);
    }
}
