using MediatR;
using AimsTransactionService.Application.Attendance.Commands.ProcessAttendanceBatch;
using AimsTransactionService.Application.Attendance.Queries.GetAttendanceSummary;

namespace AimsTransactionService.API.MinimalApis;

public static class AttendanceEndpoints
{
    public static IEndpointRouteBuilder MapAttendanceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/attendance")
            .WithTags("Attendance v2 (Minimal APIs)")
            .RequireAuthorization();

        group.MapGet("/summary/{employeeSysId:long}", async (
            long employeeSysId, DateTime monthStart, DateTime monthEnd, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetAttendanceSummaryQuery(employeeSysId, monthStart, monthEnd), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetAttendanceSummaryMinimal").WithSummary("Get attendance summary");

        group.MapPost("/batch", async (ProcessAttendanceBatchCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return Results.Created($"/api/v2/attendance/batch/{result.BatchId}", result);
        }).WithName("ProcessAttendanceBatchMinimal").WithSummary("Process attendance batch");

        return app;
    }
}
