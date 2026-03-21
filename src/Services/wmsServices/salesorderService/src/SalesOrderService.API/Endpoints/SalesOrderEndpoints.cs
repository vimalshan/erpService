using MediatR;
using SalesOrderService.Application.SalesOrders.Queries.GetAllSalesOrders;
using SalesOrderService.Application.SalesOrders.Queries.GetSalesOrderById;
using SalesOrderService.Application.SalesOrders.Queries.GetSalesOrdersByCustomer;
using SalesOrderService.Application.SalesOrders.Commands.CreateSalesOrder;
using SalesOrderService.Application.SalesOrders.Commands.ConfirmSalesOrder;
using SalesOrderService.Application.SalesOrders.Commands.CancelSalesOrder;
using Microsoft.AspNetCore.Authorization;

namespace SalesOrderService.API.Endpoints;

/// <summary>Minimal API endpoints — an alternative route surface to the controllers.</summary>
public static class SalesOrderEndpoints
{
    public static IEndpointRouteBuilder MapSalesOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/salesorders")
            .WithTags("SalesOrders (Minimal)")
            .RequireAuthorization();

        group.MapGet("/", async (ISender mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllSalesOrdersQuery(), ct)))
            .WithName("MinimalGetAllOrders")
            .Produces(200);

        group.MapGet("/{id:int}", async (int id, ISender mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSalesOrderByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinimalGetOrderById")
        .Produces(200).ProducesProblem(404);

        group.MapPost("/", async (CreateSalesOrderCommand cmd, ISender mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            return Results.Created($"/api/minimal/salesorders/{result.SoId}", result);
        })
        .WithName("MinimalCreateOrder")
        .Produces(201).ProducesValidationProblem();

        group.MapPost("/{id:int}/confirm", async (int id, ISender mediator, CancellationToken ct) =>
        {
            await mediator.Send(new ConfirmSalesOrderCommand(id), ct);
            return Results.NoContent();
        })
        .WithName("MinimalConfirmOrder")
        .Produces(204);

        return app;
    }
}
