using MediatR;
using EmployeeRelations.Application.DTOs;
using EmployeeRelations.Application.Queries.Ews;
using EmployeeRelations.Infrastructure.Persistence.Dapper;

namespace EmployeeRelations.API.MinimalApis;

public static class EwsMinimalApis
{
    public static IEndpointRouteBuilder MapEwsMinimalApis(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/ews")
            .WithTags("EWS Minimal APIs")
            .RequireAuthorization();

        group.MapGet("/dashboard/{periodNo:int}", async (int periodNo, IDapperReadRepository repo, CancellationToken ct) =>
        {
            var data = await repo.GetEwsDashboardAsync(periodNo, ct);
            return Results.Ok(data);
        })
        .WithName("GetEwsDashboard")
        .WithSummary("Get EWS dashboard data for a period via Dapper")
        .Produces<IEnumerable<EwsMainDto>>();

        group.MapGet("/active-surveys", async (IDapperReadRepository repo, CancellationToken ct) =>
        {
            var data = await repo.GetActiveSurveysAsync(ct);
            return Results.Ok(data);
        })
        .WithName("GetActiveSurveys")
        .WithSummary("Get active surveys via Dapper")
        .Produces<IEnumerable<SurveyMasterDto>>();

        group.MapGet("/disciplinary/unit/{unitId:long}", async (long unitId, IDapperReadRepository repo, CancellationToken ct) =>
        {
            var data = await repo.GetDisciplinaryCasesByUnitAsync(unitId, ct);
            return Results.Ok(data);
        })
        .WithName("GetDisciplinaryCasesByUnit")
        .WithSummary("Get disciplinary cases by unit via Dapper")
        .Produces<IEnumerable<DisciplinaryMainDto>>();

        return app;
    }
}
