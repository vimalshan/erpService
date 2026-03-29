using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.DynamicHealthDetails.Commands.Save;
using HealthTransaction.Application.Features.DynamicHealthDetails.Queries.GetByHlthNum;
using MediatR;

namespace HealthTransaction.API.MinimalApis;

public static class DynamicHealthEndpoints
{
    public static void MapDynamicHealthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/dynamic-health")
            .WithTags("DynamicHealth v2")
            .RequireAuthorization();

        group.MapGet("/{hlthNum}", async (decimal hlthNum, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetDynamicHealthDetailsByHlthNumQuery(hlthNum), ct)));

        group.MapPost("/", async (IList<SaveDynamicHealthDetailDto> items, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new SaveDynamicHealthDetailsCommand(items), ct)));
    }
}
