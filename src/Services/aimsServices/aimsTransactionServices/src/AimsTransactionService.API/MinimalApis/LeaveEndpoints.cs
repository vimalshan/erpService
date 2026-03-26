using MediatR;
using AimsTransactionService.Application.Leaves.Commands.ApplyLeave;
using AimsTransactionService.Application.Leaves.Commands.ApproveLeave;
using AimsTransactionService.Application.Leaves.Queries.GetLeavesByEmployee;
using AimsTransactionService.Application.Leaves.Queries.GetLeaveBalance;

namespace AimsTransactionService.API.MinimalApis;

public static class LeaveEndpoints
{
    public static IEndpointRouteBuilder MapLeaveEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/leaves")
            .WithTags("Leaves v2 (Minimal APIs)")
            .RequireAuthorization();

        group.MapGet("/employee/{employeeSysId:long}", async (
            long employeeSysId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetLeavesByEmployeeQuery(employeeSysId), ct);
            return Results.Ok(result);
        }).WithName("GetLeavesByEmployeeMinimal").WithSummary("Get leaves by employee");

        group.MapGet("/balance/{employeeSysId:long}/{leaveId:int}", async (
            long employeeSysId, int leaveId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetLeaveBalanceQuery(employeeSysId, leaveId), ct);
            return Results.Ok(result);
        }).WithName("GetLeaveBalanceMinimal").WithSummary("Get leave balance");

        group.MapPost("/", async (ApplyLeaveCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return Results.Created($"/api/v2/leaves/{result.LeaveDetailId}", result);
        }).WithName("ApplyLeaveMinimal").WithSummary("Apply for leave");

        group.MapPost("/{id:long}/approve", async (long id, ApproveLeaveCommand command, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(command with { LeaveDetailId = id }, ct);
            return Results.NoContent();
        }).WithName("ApproveLeaveMinimal").WithSummary("Approve or reject leave");

        return app;
    }
}
