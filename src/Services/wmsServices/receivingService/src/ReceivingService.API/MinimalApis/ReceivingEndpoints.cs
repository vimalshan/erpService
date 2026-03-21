using MediatR;
using ReceivingService.Application.Commands.CreateReceiving;
using ReceivingService.Application.Queries.GetReceivingById;
using ReceivingService.Application.Queries.GetAllReceivings;

namespace ReceivingService.API.MinimalApis;

/// <summary>Minimal-API endpoint definitions – an alternative to the MVC controller.</summary>
public static class ReceivingEndpoints
{
    public static WebApplication MapReceivingEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/minimal/receivings")
                       .WithTags("Receivings (Minimal API)")
                       .RequireAuthorization();

        group.MapGet("/", async (
            IMediator mediator,
            int page     = 1,
            int pageSize = 20,
            CancellationToken ct = default)
            => Results.Ok(await mediator.Send(new GetAllReceivingsQuery(page, pageSize), ct)));

        group.MapGet("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetReceivingByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async (
            CreateReceivingCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v1/minimal/receivings/{result.Id}", result);
        });

        return app;
    }
}
