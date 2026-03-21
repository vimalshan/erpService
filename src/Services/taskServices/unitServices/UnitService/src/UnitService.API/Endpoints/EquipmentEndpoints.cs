using MediatR;
using UnitService.Application.Commands.RegisterEquipment;
using UnitService.Application.Commands.UpdateEquipmentStatus;
using UnitService.Application.DTOs;
using UnitService.Application.Queries.GetAllEquipment;
using UnitService.Application.Queries.GetEquipment;
using UnitService.Application.Queries.GetEquipmentStatus;

namespace UnitService.API.Endpoints;

public static class EquipmentEndpoints
{
    public static void MapEquipmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/equipment")
            .WithTags("Equipment (Minimal)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllEquipmentQuery());
            return Results.Ok(result);
        }).WithName("GetAllEquipmentMinimal");

        group.MapGet("/{id:int}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetEquipmentQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetEquipmentByIdMinimal");

        group.MapGet("/{id:int}/statuses", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetEquipmentStatusQuery(id));
            return Results.Ok(result);
        }).WithName("GetEquipmentStatusesMinimal");

        group.MapPost("/", async (RegisterEquipmentCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/minimal/equipment/{result}", result);
        }).WithName("RegisterEquipmentMinimal");

        group.MapPost("/{id:int}/status", async (int id, UpdateEquipmentStatusCommand command, IMediator mediator) =>
        {
            if (id != command.EquipmentId)
                return Results.BadRequest("Equipment ID mismatch.");

            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).WithName("UpdateEquipmentStatusMinimal");
    }
}
