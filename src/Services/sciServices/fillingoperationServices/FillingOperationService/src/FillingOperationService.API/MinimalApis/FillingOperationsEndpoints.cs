using FillingOperationService.Application.FillingPlants.Commands.CreateFillingPlant;
using FillingOperationService.Application.FillingPlants.Queries.GetFillingPlantById;
using FillingOperationService.Application.FillingPlants.Queries.GetFillingPlants;
using MediatR;

namespace FillingOperationService.API.MinimalApis;

public static class FillingOperationsEndpoints
{
    public static void MapFillingOperationsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/fillingoperations")
            .RequireAuthorization()
            .WithTags("FillingOperations Minimal");

        group.MapGet("/plants", async (IMediator mediator, int? companyUnitId, CancellationToken ct) =>
        {
            var plants = await mediator.Send(new GetFillingPlantsQuery(companyUnitId), ct);
            return Results.Ok(plants);
        }).WithName("GetAllPlantsMinimal");

        group.MapGet("/plants/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var plant = await mediator.Send(new GetFillingPlantByIdQuery(id), ct);
            return plant is null ? Results.NotFound() : Results.Ok(plant);
        }).WithName("GetPlantByIdMinimal");

        group.MapPost("/plants", async (CreateFillingPlantCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/fillingoperations/plants/{id}", new { id });
        }).WithName("CreatePlantMinimal");
    }
}
