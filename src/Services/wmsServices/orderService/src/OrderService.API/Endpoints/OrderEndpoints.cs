using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.DTOs;
using OrderService.Application.Queries;
using OrderService.Infrastructure.Repositories;

namespace OrderService.API.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/orders")
            .WithTags("Orders (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllOrdersQuery(), ct)));

        group.MapGet("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var order = await mediator.Send(new GetOrderByIdQuery(id), ct);
            return order == null ? Results.NotFound() : Results.Ok(order);
        });

        group.MapPost("/", async (CreateOrderRequest request, IMediator mediator, CancellationToken ct) =>
        {
            var order = await mediator.Send(new CreateOrderCommand(request), ct);
            return Results.Created($"/api/minimal/orders/{order.OrderId}", order);
        });

        group.MapPut("/{id:int}/status", async (int id, UpdateOrderStatusRequest request, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new UpdateOrderStatusCommand(id, request.Status), ct);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteOrderCommand(id), ct);
            return Results.NoContent();
        });

        // Dapper paged endpoint
        group.MapGet("/paged", async (int page, int pageSize, OrderDapperRepository repo, CancellationToken ct) =>
        {
            var orders = await repo.GetOrdersPagedAsync(page, pageSize, ct);
            var total = await repo.GetOrderCountAsync(ct);
            return Results.Ok(new { items = orders, total, page, pageSize });
        });
    }
}
