using MediatR;
using WMTransactional.Application.Commands.CreatePurchaseOrder;
using WMTransactional.Application.Commands.ConfirmPurchaseOrder;
using WMTransactional.Application.Commands.CancelPurchaseOrder;
using WMTransactional.Application.Commands.CreateReceiving;
using WMTransactional.Application.Commands.CloseReceiving;
using WMTransactional.Application.Commands.CreateSalesOrder;
using WMTransactional.Application.Commands.ConfirmSalesOrder;
using WMTransactional.Application.Commands.CancelSalesOrder;
using WMTransactional.Application.Commands.CreateShipment;
using WMTransactional.Application.Commands.ShipShipment;
using WMTransactional.Application.Queries.GetPurchaseOrder;
using WMTransactional.Application.Queries.GetPurchaseOrders;
using WMTransactional.Application.Queries.GetReceiving;
using WMTransactional.Application.Queries.GetReceivings;
using WMTransactional.Application.Queries.GetSalesOrder;
using WMTransactional.Application.Queries.GetSalesOrders;
using WMTransactional.Application.Queries.GetShipment;
using WMTransactional.Application.Queries.GetShipments;

namespace WMTransactional.API.Endpoints;

public static class TransactionalEndpoints
{
    public static void MapTransactionalEndpoints(this IEndpointRouteBuilder app)
    {
        // Purchase Order Endpoints
        var poGroup = app.MapGroup("/api/minimal/purchase-orders")
            .WithTags("Purchase Orders (Minimal API)")
            .RequireAuthorization();

        poGroup.MapGet("/{id:int}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetPurchaseOrderQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinimalGetPurchaseOrder");

        poGroup.MapGet("/", async (string? status, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetPurchaseOrdersQuery { Status = status });
            return Results.Ok(result);
        })
        .WithName("MinimalGetPurchaseOrders");

        poGroup.MapPost("/", async (CreatePurchaseOrderCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/minimal/purchase-orders/{result.PoId}", result);
        })
        .WithName("MinimalCreatePurchaseOrder");

        poGroup.MapPut("/{id:int}/confirm", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new ConfirmPurchaseOrderCommand(id));
            return Results.NoContent();
        })
        .WithName("MinimalConfirmPurchaseOrder");

        poGroup.MapPut("/{id:int}/cancel", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new CancelPurchaseOrderCommand(id));
            return Results.NoContent();
        })
        .WithName("MinimalCancelPurchaseOrder");

        // Receiving Endpoints
        var recGroup = app.MapGroup("/api/minimal/receivings")
            .WithTags("Receivings (Minimal API)")
            .RequireAuthorization();

        recGroup.MapGet("/{id:int}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetReceivingQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinimalGetReceiving");

        recGroup.MapGet("/", async (int? purchaseOrderId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetReceivingsQuery { PoId = purchaseOrderId });
            return Results.Ok(result);
        })
        .WithName("MinimalGetReceivings");

        recGroup.MapPost("/", async (CreateReceivingCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/minimal/receivings/{result.ReceivingId}", result);
        })
        .WithName("MinimalCreateReceiving");

        recGroup.MapPut("/{id:int}/close", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new CloseReceivingCommand(id));
            return Results.NoContent();
        })
        .WithName("MinimalCloseReceiving");

        // Sales Order Endpoints
        var soGroup = app.MapGroup("/api/minimal/sales-orders")
            .WithTags("Sales Orders (Minimal API)")
            .RequireAuthorization();

        soGroup.MapGet("/{id:int}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetSalesOrderQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinimalGetSalesOrder");

        soGroup.MapGet("/", async (string? status, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetSalesOrdersQuery { Status = status });
            return Results.Ok(result);
        })
        .WithName("MinimalGetSalesOrders");

        soGroup.MapPost("/", async (CreateSalesOrderCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/minimal/sales-orders/{result.SoId}", result);
        })
        .WithName("MinimalCreateSalesOrder");

        soGroup.MapPut("/{id:int}/confirm", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new ConfirmSalesOrderCommand(id));
            return Results.NoContent();
        })
        .WithName("MinimalConfirmSalesOrder");

        soGroup.MapPut("/{id:int}/cancel", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new CancelSalesOrderCommand(id));
            return Results.NoContent();
        })
        .WithName("MinimalCancelSalesOrder");

        // Shipment Endpoints
        var shipGroup = app.MapGroup("/api/minimal/shipments")
            .WithTags("Shipments (Minimal API)")
            .RequireAuthorization();

        shipGroup.MapGet("/{id:int}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetShipmentQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinimalGetShipment");

        shipGroup.MapGet("/", async (int? salesOrderId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetShipmentsQuery { SoId = salesOrderId });
            return Results.Ok(result);
        })
        .WithName("MinimalGetShipments");

        shipGroup.MapPost("/", async (CreateShipmentCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/minimal/shipments/{result.ShipmentId}", result);
        })
        .WithName("MinimalCreateShipment");

        shipGroup.MapPut("/{id:int}/ship", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new ShipShipmentCommand(id));
            return Results.NoContent();
        })
        .WithName("MinimalShipShipment");
    }
}
