using BusServices.Application.Buses.Queries;
using BusServices.Infrastructure.Persistence.Dapper;
using MediatR;

namespace BusServices.API.MinimalApis;

public static class BusEndpoints
{
    public static WebApplication MapBusEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/buses")
            .WithTags("Buses (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetBusesQuery(), ct)))
            .WithName("GetAllBusesV2")
            .WithSummary("Get all buses (Minimal API version)");

        group.MapGet("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetBusByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetBusByIdV2");

        group.MapGet("/reports/summary", async (BusDapperQueries dapper, CancellationToken ct) =>
            Results.Ok(await dapper.GetBusSummaryAsync(ct)))
            .WithName("GetBusSummary")
            .WithSummary("Get bus summary report using Dapper");

        group.MapGet("/reports/arrivals", async (DateTime from, DateTime to, BusDapperQueries dapper, CancellationToken ct) =>
            Results.Ok(await dapper.GetArrivalReportAsync(from, to, ct)))
            .WithName("GetArrivalReport");

        group.MapGet("/reports/employees", async (BusDapperQueries dapper, CancellationToken ct) =>
            Results.Ok(await dapper.GetEmployeeBusReportAsync(ct)))
            .WithName("GetEmployeeBusReport");

        return app;
    }
}
