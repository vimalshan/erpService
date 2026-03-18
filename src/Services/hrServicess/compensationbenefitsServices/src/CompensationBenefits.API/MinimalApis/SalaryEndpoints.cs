using CompensationBenefits.Application.Features.Salaries.Queries;
using CompensationBenefits.Application.Features.SalaryStructures;
using MediatR;

namespace CompensationBenefits.API.MinimalApis;

public static class SalaryEndpoints
{
    public static WebApplication MapSalaryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/salaries")
            .WithTags("Salaries (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllSalariesQuery(), ct)));

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSalaryByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        return app;
    }

    public static WebApplication MapSalaryStructureEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/salary-structures")
            .WithTags("SalaryStructures (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllSalaryStructuresQuery(), ct)));

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSalaryStructureByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        return app;
    }
}
