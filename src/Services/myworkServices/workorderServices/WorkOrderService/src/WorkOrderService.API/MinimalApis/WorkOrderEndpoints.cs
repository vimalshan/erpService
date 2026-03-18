using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkOrderService.Application.Commands.AssignTask;
using WorkOrderService.Application.Commands.CompleteTask;
using WorkOrderService.Application.Commands.CreateWorkOrder;
using WorkOrderService.Application.Queries.GetAllWorkOrders;
using WorkOrderService.Application.Queries.GetTasksByWorkOrder;
using WorkOrderService.Application.Queries.GetWorkOrder;

namespace WorkOrderService.API.MinimalApis;

public static class WorkOrderEndpoints
{
    public static void MapWorkOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/workorders")
            .WithTags("WorkOrders-MinimalAPI")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllWorkOrdersQuery(), ct);
            return Results.Ok(result);
        }).WithName("GetAllWorkOrdersV2");

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetWorkOrderQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetWorkOrderByIdV2");

        group.MapPost("/", async ([FromBody] CreateWorkOrderCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/workorders/{result.WorkOrderId}", result);
        }).WithName("CreateWorkOrderV2");

        group.MapGet("/{workOrderId:long}/tasks", async (long workOrderId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTasksByWorkOrderQuery(workOrderId), ct);
            return Results.Ok(result);
        }).WithName("GetTasksByWorkOrderV2");

        group.MapPost("/tasks", async ([FromBody] AssignTaskCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created(string.Empty, result);
        }).WithName("AssignTaskV2");

        group.MapPut("/tasks/complete", async ([FromBody] CompleteTaskCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithName("CompleteTaskV2");
    }
}
