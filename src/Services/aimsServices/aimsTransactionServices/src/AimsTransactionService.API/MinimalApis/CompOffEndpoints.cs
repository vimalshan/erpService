using MediatR;
using AimsTransactionService.Application.CompOffs.Commands.RequestCompOff;
using AimsTransactionService.Application.CompOffs.Queries.GetCompOffsByEmployee;

namespace AimsTransactionService.API.MinimalApis;

public static class CompOffEndpoints
{
    public static IEndpointRouteBuilder MapCompOffEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/compoffs")
            .WithTags("CompOffs v2 (Minimal APIs)")
            .RequireAuthorization();

        group.MapGet("/employee/{employeeSysId:long}", async (
            long employeeSysId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetCompOffsByEmployeeQuery(employeeSysId), ct);
            return Results.Ok(result);
        }).WithName("GetCompOffsByEmployeeMinimal").WithSummary("Get comp offs by employee");

        group.MapPost("/", async (RequestCompOffCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return Results.Created($"/api/v2/compoffs/{result.CompOffId}", result);
        }).WithName("RequestCompOffMinimal").WithSummary("Request comp off");

        return app;
    }
}
