using CalendarService.Infrastructure.Dapper;
using Microsoft.AspNetCore.Authorization;

namespace CalendarService.API.MinimalApis;

public static class ReportEndpoints
{
    public static IEndpointRouteBuilder MapReportEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reports").RequireAuthorization();

        group.MapGet("/shifts/summary", async (DapperReadService svc) =>
            Results.Ok(await svc.GetShiftSummariesAsync()))
            .WithName("ShiftSummary")
            .WithTags("Reports");

        group.MapGet("/holidays/upcoming", async (DapperReadService svc, int days = 30) =>
            Results.Ok(await svc.GetUpcomingHolidaysAsync(days)))
            .WithName("UpcomingHolidays")
            .WithTags("Reports");

        group.MapGet("/calendars/summary", async (DapperReadService svc) =>
            Results.Ok(await svc.GetCalendarSummariesAsync()))
            .WithName("CalendarSummary")
            .WithTags("Reports");

        return app;
    }
}
