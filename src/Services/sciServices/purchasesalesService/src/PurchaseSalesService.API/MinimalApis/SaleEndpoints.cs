using MediatR;
using PurchaseSalesService.Application.Sales.Commands.CancelSale;
using PurchaseSalesService.Application.Sales.Commands.CreateSale;
using PurchaseSalesService.Application.Sales.Queries.GetAllSales;
using PurchaseSalesService.Application.Sales.Queries.GetSaleById;

namespace PurchaseSalesService.API.MinimalApis;

public static class SaleEndpoints
{
    public static void MapSaleEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/sales")
            .WithTags("Sales v2 (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetAllSalesQuery(), ct)))
            .WithName("GetAllSalesMin");

        group.MapGet("/{serialNumber:long}", async (long serialNumber, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSaleByIdQuery(serialNumber), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetSaleByIdMin");

        group.MapPost("/", async (CreateSaleCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/sales/{result.SerialNumber}", result);
        }).WithName("CreateSaleMin");

        group.MapPatch("/{serialNumber:long}/cancel", async (long serialNumber, string cancelledBy, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new CancelSaleCommand(serialNumber, cancelledBy), ct);
            return Results.NoContent();
        }).WithName("CancelSaleMin");
    }
}
