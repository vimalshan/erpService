using InvoiceProcessing.Application.DTOs;
using InvoiceProcessing.Application.Features.Documents.Commands;
using InvoiceProcessing.Application.Features.Documents.Queries;
using MediatR;

namespace InvoiceProcessing.API.Endpoints;

public static class DocumentEndpoints
{
    public static void MapDocumentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/documents").WithTags("Documents (Minimal API)");

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllDocumentsQuery(), ct);
            return Results.Ok(result);
        }).WithName("GetAllDocumentsMinimal");

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDocumentByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDocumentByIdMinimal");

        group.MapGet("/paged", async (int page, int pageSize, string? orgId, string? status, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPagedDocumentsQuery(page, pageSize, orgId, status), ct);
            return Results.Ok(result);
        }).WithName("GetPagedDocumentsMinimal");

        group.MapPost("/", async (CreateDocumentCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/documents/{result.DocId}", result);
        }).WithName("CreateDocumentMinimal");

        group.MapPost("/{id:long}/submit", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new SubmitDocumentCommand(id), ct);
            return Results.Ok(result);
        }).WithName("SubmitDocumentMinimal");

        group.MapPost("/{id:long}/approve", async (long id, long approvedBy, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new ApproveDocumentCommand(id, approvedBy), ct);
            return Results.Ok(result);
        }).WithName("ApproveDocumentMinimal");

        group.MapDelete("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new DeleteDocumentCommand(id), ct);
            return result ? Results.NoContent() : Results.NotFound();
        }).WithName("DeleteDocumentMinimal");
    }
}
