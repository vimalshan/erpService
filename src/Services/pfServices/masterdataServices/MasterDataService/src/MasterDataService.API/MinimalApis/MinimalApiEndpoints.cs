using MasterDataService.Application.DTOs;
using MasterDataService.Application.Features.LovMaster.Commands;
using MasterDataService.Application.Features.LovMaster.Queries;
using MasterDataService.Application.Features.Configuration.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MasterDataService.API.MinimalApis;

public static class MinimalApiEndpoints
{
    public static WebApplication MapMinimalApiEndpoints(this WebApplication app)
    {
        var masterData = app.MapGroup("/minimal/masterdata")
            .WithTags("MasterData Minimal APIs")
            .RequireAuthorization();

        masterData.MapGet("/lov", async ([FromQuery] string? category, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllLovQuery(category), ct);
            return Results.Ok(result);
        })
        .WithName("MinimalGetLov");

        masterData.MapGet("/lov/{id:decimal}", async (decimal id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLovByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinimalGetLovById");

        masterData.MapPost("/lov", async ([FromBody] CreateLovMasterDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new CreateLovCommand(dto.LovCode, dto.LovDescription, dto.LovValue, dto.LovCategory);
            var result = await mediator.Send(command, ct);
            return Results.Created($"/minimal/masterdata/lov/{result.LovId}", result);
        })
        .WithName("MinimalCreateLov");

        masterData.MapGet("/configurations", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllConfigurationsQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("MinimalGetConfigurations");

        masterData.MapGet("/configurations/{key}", async (string key, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetConfigurationByKeyQuery(key), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinimalGetConfigurationByKey");

        return app;
    }
}
