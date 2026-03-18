using MediatR;
using LovService.Application.Features.LovMaster.Queries;
using LovService.Application.Features.LovTypeMast.Queries;
using LovService.Application.Features.ProgramLovMast.Queries;
using LovService.Infrastructure.Repositories;

namespace LovService.API.MinimalApis;

/// <summary>Minimal API endpoints as lightweight alternative to controllers.</summary>
public static class LovMinimalEndpoints
{
    public static WebApplication MapLovEndpoints(this WebApplication app)
    {
        var lov = app.MapGroup("/api/v1/lov").RequireAuthorization();

        lov.MapGet("/types", async (IMediator m, int? orgId, CancellationToken ct)
            => Results.Ok(await m.Send(new GetAllLovTypesQuery(orgId), ct)))
            .WithName("minimal-get-lov-types")
            .WithTags("LOV (Minimal API)");

        lov.MapGet("/masters/{lovTypeId:int}", async (IMediator m, int lovTypeId, CancellationToken ct)
            => Results.Ok(await m.Send(new GetAllLovMastersQuery(lovTypeId), ct)))
            .WithName("minimal-get-lov-masters")
            .WithTags("LOV (Minimal API)");

        lov.MapGet("/programs/{typeCode}", async (IMediator m, string typeCode, CancellationToken ct)
            => Results.Ok(await m.Send(new GetAllProgramLovsQuery(typeCode), ct)))
            .WithName("minimal-get-program-lovs")
            .WithTags("LOV (Minimal API)");

        lov.MapGet("/dapper/masters/{lovTypeId:int}",
            async (LovDapperRepository dapper, int lovTypeId, CancellationToken ct)
                => Results.Ok(await dapper.GetLovMastersByTypeIdAsync(lovTypeId, ct)))
            .WithName("minimal-dapper-lov-masters")
            .WithTags("LOV (Minimal API - Dapper)");

        return app;
    }
}
