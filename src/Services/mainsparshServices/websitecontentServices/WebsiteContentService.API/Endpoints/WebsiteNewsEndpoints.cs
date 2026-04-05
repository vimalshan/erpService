namespace WebsiteContentService.API.Endpoints;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebsiteContentService.Application.Commands.News;
using WebsiteContentService.Application.DTOs;
using WebsiteContentService.Application.Queries.News;

public static class WebsiteNewsEndpoints
{
    public static IEndpointRouteBuilder MapWebsiteNewsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/news")
            .WithTags("WebsiteNews-Minimal")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllWebsiteNewsQuery(), ct)));

        group.MapGet("/published", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetPublishedWebsiteNewsQuery(), ct)))
            .AllowAnonymous();

        group.MapGet("/featured", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetFeaturedWebsiteNewsQuery(), ct)))
            .AllowAnonymous();

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            try { return Results.Ok(await mediator.Send(new GetWebsiteNewsByIdQuery(id), ct)); }
            catch (KeyNotFoundException) { return Results.NotFound(); }
        });

        group.MapGet("/category/{category}", async (string category, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetWebsiteNewsByCategoryQuery(category), ct)));

        group.MapPost("/", async ([FromBody] CreateWebsiteNewsCommand command, IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var news = await mediator.Send(command, ct);
                return Results.Created($"/api/minimal/news/{news.NewsId}", news);
            }
            catch (InvalidOperationException ex) { return Results.Conflict(new { message = ex.Message }); }
        });

        group.MapPut("/{id:long}", async (long id, [FromBody] UpdateWebsiteNewsCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.NewsId) return Results.BadRequest(new { message = "ID mismatch." });
            try { return Results.Ok(await mediator.Send(command, ct)); }
            catch (KeyNotFoundException) { return Results.NotFound(); }
        });

        group.MapPatch("/{id:long}/publish", async (long id, [FromBody] PublishWebsiteNewsCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.NewsId) return Results.BadRequest(new { message = "ID mismatch." });
            try { return Results.Ok(await mediator.Send(command, ct)); }
            catch (KeyNotFoundException) { return Results.NotFound(); }
        });

        group.MapPatch("/{id:long}/archive", async (long id, [FromBody] ArchiveWebsiteNewsCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.NewsId) return Results.BadRequest(new { message = "ID mismatch." });
            try
            {
                await mediator.Send(command, ct);
                return Results.NoContent();
            }
            catch (KeyNotFoundException) { return Results.NotFound(); }
        });

        group.MapPatch("/{id:long}/featured", async (long id, [FromBody] SetNewsFeaturedCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.NewsId) return Results.BadRequest(new { message = "ID mismatch." });
            try
            {
                await mediator.Send(command, ct);
                return Results.NoContent();
            }
            catch (KeyNotFoundException) { return Results.NotFound(); }
        });

        return app;
    }
}
