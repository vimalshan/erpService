namespace OrderScheduleService.API;

using MediatR;
using OrderScheduleService.Application.Commands;
using OrderScheduleService.Application.DTOs;
using OrderScheduleService.Application.Queries;

public static class MinimalApiExtensions
{
    public static void MapMinimalApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal")
            .WithName("Minimal APIs")
            .RequireAuthorization();

        // Orders Minimal APIs
        group.MapGet("/orders", GetAllOrders)
            .WithName("Get All Orders")
            .WithDescription("Retrieve all orders");

        group.MapGet("/orders/{id}", GetOrderById)
            .WithName("Get Order By ID")
            .WithDescription("Retrieve a specific order by ID");

        group.MapGet("/orders/customer/{customerCode}", GetOrdersByCustomer)
            .WithName("Get Orders By Customer")
            .WithDescription("Retrieve orders for a specific customer");

        group.MapPost("/orders", CreateOrder)
            .WithName("Create Order")
            .WithDescription("Create a new order");

        group.MapPut("/orders/{id}/status", UpdateOrderStatus)
            .WithName("Update Order Status")
            .WithDescription("Update the status of an order");

        // Schedules Minimal APIs
        group.MapGet("/schedules/{id}", GetScheduleById)
            .WithName("Get Schedule By ID")
            .WithDescription("Retrieve a specific schedule by ID");

        group.MapGet("/schedules/item/{itemId}", GetSchedulesByItem)
            .WithName("Get Schedules By Item")
            .WithDescription("Retrieve schedules for a specific item");

        group.MapPost("/schedules", CreateSchedule)
            .WithName("Create Schedule")
            .WithDescription("Create a new schedule");

        group.MapPut("/schedules/{id}/confirm", ConfirmSchedule)
            .WithName("Confirm Schedule")
            .WithDescription("Confirm a schedule");

        // Health check endpoint
        app.MapGet("/health/ready", HealthCheck)
            .WithName("Health Check")
            .WithDescription("API health check")
            .AllowAnonymous();
    }

    private static async Task<IResult> GetAllOrders(IMediator mediator, ILogger<Program> logger)
    {
        try
        {
            var orders = await mediator.Send(new GetAllOrdersQuery());
            return Results.Ok(orders);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving orders");
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> GetOrderById(long id, IMediator mediator, ILogger<Program> logger)
    {
        try
        {
            var order = await mediator.Send(new GetTiedOrderByIdQuery(id));
            return order == null ? Results.NotFound() : Results.Ok(order);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error retrieving order {id}");
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> GetOrdersByCustomer(string customerCode, IMediator mediator, ILogger<Program> logger)
    {
        try
        {
            var orders = await mediator.Send(new GetOrdersByCustomerQuery(customerCode));
            return Results.Ok(orders);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error retrieving orders for customer {customerCode}");
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> CreateOrder(CreateTiedOrderDto orderDto, IMediator mediator, ILogger<Program> logger)
    {
        try
        {
            var orderId = await mediator.Send(new CreateTiedOrderCommand(orderDto));
            return Results.Created($"/api/minimal/orders/{orderId}", new { orderId });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating order");
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> UpdateOrderStatus(long id, char status, IMediator mediator, HttpContext context, ILogger<Program> logger)
    {
        try
        {
            var userId = context.User.FindFirst("sub")?.Value ?? "SYSTEM";
            var result = await mediator.Send(new UpdateOrderStatusCommand(id, status, userId));
            return Results.Ok(new { success = result });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error updating order {id} status");
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> GetScheduleById(long id, IMediator mediator, ILogger<Program> logger)
    {
        try
        {
            var schedule = await mediator.Send(new GetScheduleByIdQuery(id));
            return schedule == null ? Results.NotFound() : Results.Ok(schedule);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error retrieving schedule {id}");
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> GetSchedulesByItem(decimal itemId, IMediator mediator, ILogger<Program> logger)
    {
        try
        {
            var schedules = await mediator.Send(new GetSchedulesByItemQuery(itemId));
            return Results.Ok(schedules);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error retrieving schedules for item {itemId}");
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> CreateSchedule(CreateScheduleDto scheduleDto, IMediator mediator, ILogger<Program> logger)
    {
        try
        {
            var scheduleId = await mediator.Send(new CreateScheduleCommand(scheduleDto));
            return Results.Created($"/api/minimal/schedules/{scheduleId}", new { scheduleId });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating schedule");
            return Results.StatusCode(500);
        }
    }

    private static async Task<IResult> ConfirmSchedule(long id, IMediator mediator, ILogger<Program> logger)
    {
        try
        {
            var result = await mediator.Send(new ConfirmScheduleCommand(id));
            return Results.Ok(new { success = result });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error confirming schedule {id}");
            return Results.StatusCode(500);
        }
    }

    private static Task<IResult> HealthCheck()
    {
        return Task.FromResult(Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));
    }
}
