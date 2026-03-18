using LeaveServices.Infrastructure.Dapper;
using LeaveServices.Application.DTOs;

namespace LeaveServices.API.MinimalApis;

public static class LeaveMinimalApis
{
    public static void MapLeaveMinimalApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/leave")
            .WithTags("Leave Minimal APIs")
            .RequireAuthorization();

        // Paginated leave requests
        group.MapGet("/requests", async (
            int page,
            int pageSize,
            ILeaveReadRepository readRepo,
            CancellationToken ct) =>
        {
            var results = await readRepo.GetAllLeaveRequestsPagedAsync(page, pageSize, ct);
            return Results.Ok(results);
        })
        .WithName("GetAllLeaveRequestsPaged")
        .WithSummary("Get paginated leave requests (Dapper read model)");

        // Encashments by status
        group.MapGet("/encashments", async (
            char? status,
            ILeaveReadRepository readRepo,
            CancellationToken ct) =>
        {
            var results = await readRepo.GetEncashmentsByStatusAsync(status, ct);
            return Results.Ok(results);
        })
        .WithName("GetEncashmentsByStatus")
        .WithSummary("Get encashments filtered by status using Dapper");

        // LOP summary by month
        group.MapGet("/lop/month/{year:int}/{month:int}", async (
            int year,
            int month,
            ILeaveReadRepository readRepo,
            CancellationToken ct) =>
        {
            var results = await readRepo.GetLopSummaryByMonthAsync(new DateOnly(year, month, 1), ct);
            return Results.Ok(results);
        })
        .WithName("GetLopSummaryByMonth")
        .WithSummary("Get LOP summary for a given month");
    }
}
