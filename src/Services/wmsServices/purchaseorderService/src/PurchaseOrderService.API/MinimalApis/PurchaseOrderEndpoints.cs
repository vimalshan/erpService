using MediatR;
using Microsoft.AspNetCore.Mvc;
using PurchaseOrderService.Application.Commands.CancelPurchaseOrder;
using PurchaseOrderService.Application.Commands.ConfirmPurchaseOrder;
using PurchaseOrderService.Application.Commands.CreatePurchaseOrder;
using PurchaseOrderService.Application.Commands.ReceivePurchaseOrderLine;
using PurchaseOrderService.Application.Queries.GetPurchaseOrderById;
using PurchaseOrderService.Application.Queries.GetPurchaseOrders;

namespace PurchaseOrderService.API.MinimalApis;

public static class PurchaseOrderEndpoints
{
    public static void MapPurchaseOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/purchase-orders")
            .WithTags("PurchaseOrders (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null) =>
        {
            var result = await mediator.Send(new GetPurchaseOrdersQuery { Page = page, PageSize = pageSize, Status = status });
            return Results.Ok(result);
        })
        .WithName("GetAllPurchaseOrdersV2")
        .Produces(200);

        group.MapGet("/{id:int}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetPurchaseOrderByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetPurchaseOrderByIdV2")
        .Produces(200)
        .Produces(404);

        group.MapPost("/", async (CreatePurchaseOrderCommand command, IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/v2/purchase-orders/{id}", id);
        })
        .WithName("CreatePurchaseOrderV2")
        .Produces(201)
        .Produces(400);

        group.MapPost("/{id:int}/confirm", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new ConfirmPurchaseOrderCommand(id));
            return Results.NoContent();
        })
        .WithName("ConfirmPurchaseOrderV2")
        .Produces(204);

        group.MapPost("/{id:int}/cancel", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new CancelPurchaseOrderCommand(id));
            return Results.NoContent();
        })
        .WithName("CancelPurchaseOrderV2")
        .Produces(204);

        group.MapPost("/{id:int}/lines/{lineNumber:int}/receive", async (int id, int lineNumber, [FromBody] ReceiveRequest request, IMediator mediator) =>
        {
            await mediator.Send(new ReceivePurchaseOrderLineCommand { PoId = id, LineNumber = lineNumber, Quantity = request.Quantity });
            return Results.NoContent();
        })
        .WithName("ReceiveLineV2")
        .Produces(204);
    }
}

public record ReceiveRequest(decimal Quantity);
