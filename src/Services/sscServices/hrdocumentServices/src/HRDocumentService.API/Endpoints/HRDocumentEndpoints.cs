using HRDocumentService.Application.Commands;
using HRDocumentService.Application.Queries;
using MediatR;

namespace HRDocumentService.API.Endpoints;

public static class HRDocumentEndpoints
{
    public static void MapHRDocumentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/documents")
            .WithTags("HR Documents v2")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllHRDocumentsQuery(), ct);
            return Results.Ok(result);
        }).WithName("GetAllDocumentsV2");

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetHRDocumentByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetDocumentByIdV2");

        group.MapGet("/status/{status}", async (string status, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetHRDocumentsByStatusQuery(status), ct);
            return Results.Ok(result);
        }).WithName("GetDocumentsByStatusV2");

        group.MapPost("/", async (CreateHRDocumentCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/documents/{result.DocId}", result);
        }).WithName("CreateDocumentV2");

        group.MapPost("/{id:long}/approve", async (long id, decimal approvedBy, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new ApproveHRDocumentCommand(id, approvedBy), ct);
            return result ? Results.NoContent() : Results.NotFound();
        }).WithName("ApproveDocumentV2");

        group.MapPost("/{id:long}/submit", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new SubmitHRDocumentCommand(id), ct);
            return result ? Results.NoContent() : Results.NotFound();
        }).WithName("SubmitDocumentV2");

        group.MapPost("/{id:long}/reject", async (long id, string rejectRemarks, decimal rejectedBy, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new RejectHRDocumentCommand(id, rejectedBy, rejectRemarks), ct);
            return result ? Results.NoContent() : Results.NotFound();
        }).WithName("RejectDocumentV2");

        group.MapPost("/{id:long}/cancel", async (long id, decimal cancelledBy, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CancelHRDocumentCommand(id, cancelledBy), ct);
            return result ? Results.NoContent() : Results.NotFound();
        }).WithName("CancelDocumentV2");
    }
}
