using MediatR;
using PurchaseSalesService.Application.Purchases.Commands.CancelPurchase;
using PurchaseSalesService.Application.Purchases.Commands.CreatePurchase;
using PurchaseSalesService.Application.Purchases.Queries.GetAllPurchases;
using PurchaseSalesService.Application.Purchases.Queries.GetPurchaseById;

namespace PurchaseSalesService.API.MinimalApis;

public static class PurchaseEndpoints
{
    public static void MapPurchaseEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/purchases")
            .WithTags("Purchases v2 (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetAllPurchasesQuery(), ct)))
            .WithName("GetAllPurchasesMin")
            .Produces(StatusCodes.Status200OK);

        group.MapGet("/{serialNumber:long}", async (long serialNumber, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPurchaseByIdQuery(serialNumber), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetPurchaseByIdMin");

        group.MapPost("/", async (CreatePurchaseCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/purchases/{result.SerialNumber}", result);
        }).WithName("CreatePurchaseMin");

        group.MapPatch("/{serialNumber:long}/cancel", async (long serialNumber, string cancelledBy, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new CancelPurchaseCommand(serialNumber, cancelledBy), ct);
            return Results.NoContent();
        }).WithName("CancelPurchaseMin");
    }
}
