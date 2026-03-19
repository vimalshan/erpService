using FilingAndArchiveService.Application.Files.Commands.CreateFile;
using FilingAndArchiveService.Application.Files.Commands.DispatchFile;
using FilingAndArchiveService.Application.Files.Queries.GetAllFiles;
using FilingAndArchiveService.Application.Files.Queries.GetFileById;
using FilingAndArchiveService.Application.Files.Queries.GetFilesByOrg;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FilingAndArchiveService.API.MinimalApis;

public static class FilingEndpoints
{
    public static IEndpointRouteBuilder MapFilingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/files")
            .WithTags("FilingArchive-Minimal")
            .RequireAuthorization();

        group.MapGet("/", async ([FromServices] IMediator mediator, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
            => Results.Ok(await mediator.Send(new GetAllFilesQuery(page, pageSize), ct)))
            .WithName("GetAllFilesMinimal")
            .WithSummary("Get all files (minimal API)")
            .Produces(200);

        group.MapGet("/{id:long}", async ([FromServices] IMediator mediator, long id, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetFileByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetFileByIdMinimal")
        .Produces(200).Produces(404);

        group.MapGet("/org/{orgId}", async ([FromServices] IMediator mediator, string orgId, [FromQuery] long? year, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetFilesByOrgQuery(orgId, year), ct)))
            .WithName("GetFilesByOrgMinimal");

        group.MapPost("/", async ([FromServices] IMediator mediator, [FromBody] CreateFileCommand command, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/files/{result.FileId}", result);
        })
            .WithName("CreateFileMinimal")
            .Produces(201).Produces(400);

        group.MapPost("/{id:long}/dispatch", async ([FromServices] IMediator mediator, long id, [FromBody] DispatchFileCommand command, CancellationToken ct)
            => Results.Ok(await mediator.Send(command with { FileId = id }, ct)))
            .WithName("DispatchFileMinimal")
            .Produces(200).Produces(404);

        return app;
    }
}
