using MediatR;
using AimsTransactionService.Application.Swipes.Commands.RecordSwipe;
using AimsTransactionService.Application.Swipes.Queries.GetSwipesByEmployee;

namespace AimsTransactionService.API.MinimalApis;

public static class SwipeEndpoints
{
    public static IEndpointRouteBuilder MapSwipeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/swipes")
            .WithTags("Swipes v2 (Minimal APIs)")
            .RequireAuthorization();

        group.MapGet("/employee/{employeeSysId:long}", async (
            long employeeSysId, DateTime fromDate, DateTime toDate, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetSwipesByEmployeeQuery(employeeSysId, fromDate, toDate), ct);
            return Results.Ok(result);
        }).WithName("GetSwipesByEmployeeMinimal").WithSummary("Get swipes by employee");

        group.MapPost("/", async (RecordSwipeCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return Results.Created($"/api/v2/swipes/{result.SwipeId}", result);
        }).WithName("RecordSwipeMinimal").WithSummary("Record a swipe punch");

        return app;
    }
}
