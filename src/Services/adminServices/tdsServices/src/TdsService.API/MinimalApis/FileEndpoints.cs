using MediatR;
using TdsService.Application.DTOs;
using TdsService.Application.Files.Commands.UpdateEmailStatus;
using TdsService.Application.Files.Commands.UploadTdsFile;
using TdsService.Application.Files.Queries.GetAllTdsFiles;
using TdsService.Application.Files.Queries.GetTdsFileById;
using Microsoft.AspNetCore.Mvc;

namespace TdsService.API.MinimalApis;

public static class FileEndpoints
{
    public static IEndpointRouteBuilder MapFileEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/files")
            .WithTags("Files (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (
            [FromServices] IMediator mediator,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default) =>
        {
            var result = await mediator.Send(new GetAllTdsFilesQuery(page, pageSize), ct);
            return Results.Ok(result);
        }).Produces<PagedResult<TdsFileDto>>();

        group.MapGet("/{fileId:long}", async (
            long fileId,
            [FromServices] IMediator mediator,
            CancellationToken ct = default) =>
        {
            var result = await mediator.Send(new GetTdsFileByIdQuery(fileId), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        }).Produces<TdsFileDto>();

        group.MapPatch("/{fileId:long}/email-sent", async (
            long fileId,
            [FromServices] IMediator mediator,
            CancellationToken ct = default) =>
        {
            await mediator.Send(new UpdateEmailStatusCommand(fileId), ct);
            return Results.NoContent();
        });

        return app;
    }
}
