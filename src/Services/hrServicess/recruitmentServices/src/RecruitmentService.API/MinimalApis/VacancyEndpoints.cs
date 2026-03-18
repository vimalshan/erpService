using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitmentService.Application.Commands.Vacancies;
using RecruitmentService.Application.DTOs;
using RecruitmentService.Application.Queries.Vacancies;
using RecruitmentService.Infrastructure.Persistence.Dapper;

namespace RecruitmentService.API.MinimalApis;

public static class VacancyEndpoints
{
    public static WebApplication MapVacancyEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/vacancies")
            .WithTags("Vacancies (Minimal API)")
            .WithOpenApi();

        // GET all open vacancies
        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllVacanciesQuery(), ct)))
            .WithName("GetAllVacanciesMinimal")
            .WithSummary("Get all open vacancies (v2)");

        // GET by ID
        group.MapGet("/{id:decimal}", async (decimal id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetVacancyByIdQuery(id), ct);
            return Results.Ok(result);
        })
        .WithName("GetVacancyByIdMinimal")
        .WithSummary("Get vacancy by ID (v2)");

        // GET search via Dapper
        group.MapGet("/search", async (
            [FromQuery] string? unit,
            [FromQuery] string? designation,
            [FromQuery] decimal? locationId,
            DapperRepository dapper,
            CancellationToken ct) =>
        {
            var results = await dapper.SearchVacanciesAsync(unit, designation, locationId);
            return Results.Ok(results);
        })
        .WithName("SearchVacanciesMinimal")
        .WithSummary("Search vacancies with advanced filtering (v2, Dapper)");

        // GET dashboard counts
        group.MapGet("/dashboard/counts", async (DapperRepository dapper, CancellationToken ct) =>
            Results.Ok(await dapper.GetApplicationCountByVacancyAsync()))
            .WithName("VacancyDashboard")
            .WithSummary("Get application counts per vacancy for dashboard")
            .RequireAuthorization("HR");

        return app;
    }
}
