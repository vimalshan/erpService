namespace WebsiteContentService.API.Endpoints;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebsiteContentService.Application.Commands.Pages;
using WebsiteContentService.Application.DTOs;
using WebsiteContentService.Application.Queries.Pages;

public static class WebsitePageEndpoints
{
    public static IEndpointRouteBuilder MapWebsitePageEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/pages")
            .WithTags("WebsitePages-Minimal")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllWebsitePagesQuery(), ct)));

        group.MapGet("/published", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetPublishedWebsitePagesQuery(), ct)))
            .AllowAnonymous();

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            try { return Results.Ok(await mediator.Send(new GetWebsitePageByIdQuery(id), ct)); }
            catch (KeyNotFoundException) { return Results.NotFound(); }
        });

        group.MapGet("/code/{pageCode}", async (string pageCode, IMediator mediator, CancellationToken ct) =>
        {
            try { return Results.Ok(await mediator.Send(new GetWebsitePageByCodeQuery(pageCode), ct)); }
            catch (KeyNotFoundException) { return Results.NotFound(); }
        });

        group.MapPost("/", async ([FromBody] CreateWebsitePageCommand command, IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var page = await mediator.Send(command, ct);
                return Results.Created($"/api/minimal/pages/{page.PageId}", page);
            }
            catch (InvalidOperationException ex) { return Results.Conflict(new { message = ex.Message }); }
        });

        group.MapPut("/{id:long}", async (long id, [FromBody] UpdateWebsitePageCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.PageId) return Results.BadRequest(new { message = "ID mismatch." });
            try { return Results.Ok(await mediator.Send(command, ct)); }
            catch (KeyNotFoundException) { return Results.NotFound(); }
        });

        group.MapPatch("/{id:long}/publish", async (long id, [FromBody] PublishWebsitePageCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.PageId) return Results.BadRequest(new { message = "ID mismatch." });
            try { return Results.Ok(await mediator.Send(command, ct)); }
            catch (KeyNotFoundException) { return Results.NotFound(); }
        });

        group.MapPatch("/{id:long}/status", async (long id, [FromBody] ChangeWebsitePageStatusCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.PageId) return Results.BadRequest(new { message = "ID mismatch." });
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
