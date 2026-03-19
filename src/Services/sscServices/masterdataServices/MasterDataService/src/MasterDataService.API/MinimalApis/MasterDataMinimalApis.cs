using MediatR;
using MasterDataService.Application.Commands;
using MasterDataService.Application.DTOs;
using MasterDataService.Application.Queries;
using MasterDataService.Infrastructure.Persistence.Dapper;

namespace MasterDataService.API.MinimalApis;

public static class MasterDataMinimalApis
{
    public static WebApplication MapMasterDataMinimalApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2").RequireAuthorization();

        // LOV Master minimal API
        group.MapGet("/lov", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllLovMastersQuery(), ct)));

        group.MapGet("/lov/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLovMasterByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/lov", async (CreateLovMasterDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateLovMasterCommand(dto.LovId, dto.LovType, dto.LovName), ct);
            return Results.Created($"/api/v2/lov/{result.LovId}", result);
        });

        // Dapper-based query endpoints
        group.MapGet("/lov/type/{lovType}", async (string lovType, IDapperQueryService dapper, CancellationToken ct) =>
            Results.Ok(await dapper.GetLovValuesByTypeAsync(lovType, ct)));

        group.MapGet("/holdtype/category/{category}", async (char category, IDapperQueryService dapper, CancellationToken ct) =>
            Results.Ok(await dapper.GetHoldTypesByCategoryAsync(category, ct)));

        group.MapGet("/scanner/location/{locationId:long}", async (long locationId, IDapperQueryService dapper, CancellationToken ct) =>
            Results.Ok(await dapper.GetScannersByLocationAsync(locationId, ct)));

        // Hold Type Master
        group.MapGet("/holdtype", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllHoldTypeMastersQuery(), ct)));

        group.MapGet("/holdtype/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetHoldTypeMasterByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        // Scanner Master
        group.MapGet("/scanner", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllScannerMastersQuery(), ct)));

        group.MapGet("/scanner/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetScannerMasterByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        return app;
    }
}
