using MediatR;
using ProductionManagement.Application.Commands.ProductionPlants;
using ProductionManagement.Application.Commands.ProductionPlans;
using ProductionManagement.Application.DTOs;
using ProductionManagement.Application.Queries.ProductionPlans;
using ProductionManagement.Application.Queries.ProductionPlants;
using ProductionManagement.Infrastructure.Dapper;

namespace ProductionManagement.API.Endpoints;

public static class MinimalApiEndpoints
{
    public static void MapMinimalApiEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2").RequireAuthorization();

        // Production Plants - Minimal API using Dapper
        group.MapGet("/plants/dapper", async (IProductionDapperQueries queries) =>
        {
            var plants = await queries.GetAllPlantsAsync();
            return Results.Ok(plants);
        })
        .WithName("GetAllPlantsDapper");

        group.MapGet("/plants/dapper/{id:int}", async (int id, IProductionDapperQueries queries) =>
        {
            var plant = await queries.GetPlantByIdAsync(id);
            return plant is null ? Results.NotFound() : Results.Ok(plant);
        })
        .WithName("GetPlantByIdDapper");

        group.MapPost("/plants/dapper/register", async (CreateProductionPlantDto dto, IProductionDapperQueries queries) =>
        {
            var plantId = await queries.RegisterProductionPlantAsync(dto.CompanyUnitId, dto.PlantName, dto.Location, dto.CreatedBy);
            return Results.Created($"/api/v2/plants/dapper/{plantId}", new { PlantId = plantId });
        })
        .WithName("RegisterPlantDapper");

        // Production Plans via MediatR
        group.MapGet("/plans/{plantId:int}", async (int plantId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProductionPlansByPlantIdQuery(plantId), ct);
            return Results.Ok(result);
        })
        .WithName("GetPlansByPlantIdMinimal");

        // Health summary
        group.MapGet("/health/summary", () => Results.Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Service = "ProductionManagement"
        }))
        .WithName("HealthSummary")
        .AllowAnonymous();
    }
}
