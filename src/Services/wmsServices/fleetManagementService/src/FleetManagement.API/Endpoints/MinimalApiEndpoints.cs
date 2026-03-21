using FleetManagement.Application.Commands.Maintenance;
using FleetManagement.Application.Interfaces;
using FleetManagement.Application.Queries.FleetStatus;
using FleetManagement.Application.Queries.Vehicles;
using MediatR;

namespace FleetManagement.API.Endpoints;

public static class MinimalApiEndpoints
{
    public static void Map(WebApplication app)
    {
        var fleet = app.MapGroup("/api/minimal/fleet").RequireAuthorization();

        fleet.MapGet("/status", async (int? warehouseId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetFleetStatusQuery(warehouseId), ct)))
            .WithName("GetFleetStatusMinimal")
            .WithTags("Fleet");

        fleet.MapGet("/vehicles", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllVehiclesQuery(), ct)))
            .WithName("GetAllVehiclesMinimal")
            .WithTags("Fleet");

        fleet.MapPost("/fuel", async (LogFuelCommand command, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(command, ct)))
            .WithName("LogFuelMinimal")
            .WithTags("Fleet");

        fleet.MapPost("/maintenance", async (LogMaintenanceCommand command, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(command, ct)))
            .WithName("LogMaintenanceMinimal")
            .WithTags("Fleet");

        // Blob storage endpoint for vehicle images
        var images = app.MapGroup("/api/minimal/images").RequireAuthorization();

        images.MapPost("/vehicle/{vehicleId:int}", async (
            int vehicleId, IFormFile file, IBlobStorageService blobService, CancellationToken ct) =>
        {
            await using var stream = file.OpenReadStream();
            var url = await blobService.UploadFileAsync(
                "vehicle-images", $"{vehicleId}/{file.FileName}", stream, file.ContentType, ct);
            return Results.Ok(new { Url = url });
        })
        .WithName("UploadVehicleImage")
        .WithTags("Images")
        .DisableAntiforgery();

        images.MapGet("/vehicle/{vehicleId:int}/{fileName}", async (
            int vehicleId, string fileName, IBlobStorageService blobService, CancellationToken ct) =>
        {
            var stream = await blobService.DownloadFileAsync("vehicle-images", $"{vehicleId}/{fileName}", ct);
            return stream is null ? Results.NotFound() : Results.Stream(stream, "application/octet-stream", fileName);
        })
        .WithName("GetVehicleImage")
        .WithTags("Images");

        images.MapDelete("/vehicle/{vehicleId:int}/{fileName}", async (
            int vehicleId, string fileName, IBlobStorageService blobService, CancellationToken ct) =>
        {
            await blobService.DeleteFileAsync("vehicle-images", $"{vehicleId}/{fileName}", ct);
            return Results.NoContent();
        })
        .WithName("DeleteVehicleImage")
        .WithTags("Images");
    }
}
