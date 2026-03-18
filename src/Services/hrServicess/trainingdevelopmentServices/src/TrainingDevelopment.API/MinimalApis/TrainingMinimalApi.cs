using MediatR;
using Microsoft.AspNetCore.Authorization;
using TrainingDevelopment.Application.Features.TrainingDetails.Queries.GetTrainingDetailList;
using TrainingDevelopment.Infrastructure.Dapper;

namespace TrainingDevelopment.API.MinimalApis;

public static class TrainingMinimalApi
{
    public static IEndpointRouteBuilder MapTrainingMinimalApis(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/training")
            .WithTags("Training Minimal API")
            .RequireAuthorization();

        group.MapGet("/", async (
            ISender sender,
            decimal? employeeSysId,
            decimal? financialYear,
            string? status,
            CancellationToken ct) =>
        {
            var result = await sender.Send(
                new GetTrainingDetailListQuery(employeeSysId, financialYear, status), ct);
            return Results.Ok(result);
        })
        .WithName("GetTrainingsV2")
        .WithSummary("Get all training records (Minimal API)")
        .Produces(200)
        .RequireAuthorization();

        group.MapGet("/report", async (
            TrainingDetailDapperRepository dapperRepo,
            decimal? financialYear,
            string? status,
            CancellationToken ct) =>
        {
            var result = await dapperRepo.GetTrainingReportAsync(financialYear, status, ct);
            return Results.Ok(result);
        })
        .WithName("GetTrainingReport")
        .WithSummary("Get training report via Dapper (optimized read)");

        group.MapGet("/summary", async (
            TrainingDetailDapperRepository dapperRepo,
            CancellationToken ct) =>
        {
            var result = await dapperRepo.GetTrainingSummaryByStatusAsync(ct);
            return Results.Ok(result);
        })
        .WithName("GetTrainingSummary")
        .WithSummary("Get training summary by status");

        return app;
    }
}
