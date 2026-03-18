using MediatR;
using VisitorServices.Application.Approvals.Commands.ProcessApproval;
using VisitorServices.Application.Visitors.Commands.AddVisitorItem;
using VisitorServices.Application.Visitors.Commands.CheckoutVisitor;
using VisitorServices.Application.Visitors.Commands.RegisterVisitor;
using VisitorServices.Application.Visitors.Queries.GetActiveVisitors;
using VisitorServices.Application.Visitors.Queries.GetVisitorById;

namespace VisitorServices.API.MinimalApis;

public static class VisitorEndpoints
{
    public static IEndpointRouteBuilder MapVisitorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/visitors")
            .WithTags("Visitors v2 (Minimal APIs)")
            .RequireAuthorization();

        group.MapGet("/active", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetActiveVisitorsQuery(), ct);
            return Results.Ok(result);
        }).WithName("GetActiveVisitorsMinimal").WithSummary("Get active visitors");

        group.MapGet("/{id:long}", async (long id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetVisitorByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetVisitorByIdMinimal").WithSummary("Get visitor by ID");

        group.MapPost("/", async (RegisterVisitorCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return Results.Created($"/api/v2/visitors/{result.VisitorId}", result);
        }).WithName("RegisterVisitorMinimal").WithSummary("Register a visitor");

        group.MapPost("/{id:long}/checkout", async (long id, long checkedOutBy, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new CheckoutVisitorCommand(id, checkedOutBy), ct);
            return Results.NoContent();
        }).WithName("CheckoutVisitorMinimal").WithSummary("Checkout a visitor");

        group.MapPost("/{id:long}/items", async (long id, AddVisitorItemCommand command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command with { VisitorId = id }, ct);
            return Results.Created($"/api/v2/visitors/{id}/items/{result.ItemId}", result);
        }).WithName("AddVisitorItemMinimal").WithSummary("Add item to visitor");

        return app;
    }
}
