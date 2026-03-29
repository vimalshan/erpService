using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.CheckupCards.Commands.Create;
using HealthTransaction.Application.Features.CheckupCards.Queries.GetAll;
using HealthTransaction.Application.Features.CheckupCards.Queries.GetByHlthNum;
using MediatR;

namespace HealthTransaction.API.MinimalApis;

public static class CheckupCardEndpoints
{
    public static void MapCheckupCardEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/checkup-cards")
            .WithTags("CheckupCards v2")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllCheckupCardsQuery(), ct)));

        group.MapGet("/{hlthNum}", async (decimal hlthNum, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCheckupCardByHlthNumQuery(hlthNum), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async (CreateCheckupCardDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateCheckupCardCommand(dto), ct);
            return Results.Created($"/api/v2/checkup-cards/{result.HlthNum}", result);
        });
    }
}
