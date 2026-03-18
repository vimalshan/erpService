using MediatR;
using Microsoft.AspNetCore.Mvc;
using CardManagement.Application.Cards.Queries.GetGuestCards;
using CardManagement.Application.Cards.Queries.GetGuestCardById;

namespace CardManagement.API.Endpoints;

public static class CardEndpoints
{
    public static WebApplication MapCardEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/cards")
            .RequireAuthorization()
            .WithTags("Cards (Minimal API)");

        group.MapGet("/", async ([FromServices] IMediator mediator, int pageNumber = 1, int pageSize = 20, long? canteenUnit = null, CancellationToken ct = default)
            => Results.Ok(await mediator.Send(new GetGuestCardsQuery(pageNumber, pageSize, canteenUnit), ct)))
            .WithSummary("List guest cards");

        group.MapGet("/{canteenUnit:long}", async ([FromServices] IMediator mediator, long canteenUnit, CancellationToken ct = default) =>
        {
            var result = await mediator.Send(new GetGuestCardByIdQuery(canteenUnit), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithSummary("Get guest card by canteen unit");

        return app;
    }
}
