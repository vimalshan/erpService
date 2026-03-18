using Microsoft.AspNetCore.Mvc;
using MediatR;
using CompetencyService.Application.DTOs;
using CompetencyService.Infrastructure.DapperQueries;

namespace CompetencyService.API.MinimalApis;

public static class CompetencyMinimalApis
{
    public static void MapCompetencyEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/competencies")
            .WithTags("Competencies v2")
            .RequireAuthorization();

        group.MapGet("/search", async (
            [FromQuery] string term,
            [FromServices] CompetencyDapperQueries dapper) =>
        {
            var results = await dapper.SearchCompetenciesAsync(term);
            return Results.Ok(results);
        })
        .WithName("SearchCompetencies")
        .Produces<IEnumerable<CompetencyDto>>(200);

        group.MapGet("/indicators/{competencyId:decimal}", async (
            decimal competencyId,
            [FromServices] CompetencyDapperQueries dapper) =>
        {
            var results = await dapper.GetIndicatorsByCompetencyAsync(competencyId);
            return Results.Ok(results);
        })
        .WithName("GetIndicators")
        .Produces<IEnumerable<CompetencyIndicatorDto>>(200);

        group.MapGet("/paged", async (
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromServices] CompetencyDapperQueries dapper) =>
        {
            var results = await dapper.GetCompetenciesPagedAsync(page, pageSize);
            return Results.Ok(results);
        })
        .WithName("GetCompetenciesPaged")
        .Produces<IEnumerable<CompetencyDto>>(200);
    }
}
