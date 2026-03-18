using BusServices.Application.Buses.Queries;
using BusServices.Application.Arrivals.Queries;
using BusServices.Application.DTOs;
using BusServices.Application.Routes.Queries;
using BusServices.Application.EmployeeAssignments.Queries;
using MediatR;

namespace BusServices.API.GraphQL;

public sealed class BusQuery
{
    public async Task<IEnumerable<BusDto>> GetBuses([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetBusesQuery(), ct);

    public async Task<BusDto?> GetBusById(int busId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetBusByIdQuery(busId), ct);

    public async Task<IEnumerable<BusRouteDto>> GetRoutesByBus(int busId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetRoutesByBusQuery(busId), ct);

    public async Task<IEnumerable<BusArrivalDto>> GetArrivalsByBus(int busId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetArrivalsByBusQuery(busId), ct);

    public async Task<IEnumerable<BusArrivalDto>> GetArrivalsByDate(DateTime date, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetArrivalsByDateQuery(date), ct);

    public async Task<IEnumerable<EmployeeBusDto>> GetAssignmentsByEmployee(long empSysId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAssignmentsByEmployeeQuery(empSysId), ct);
}
