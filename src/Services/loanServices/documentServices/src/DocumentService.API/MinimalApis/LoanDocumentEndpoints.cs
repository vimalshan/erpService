using MediatR;
using Microsoft.AspNetCore.Authorization;
using DocumentService.Application.Commands.CreateLoanDocument;
using DocumentService.Application.Commands.DeleteLoanDocument;
using DocumentService.Application.Commands.UpdateLoanDocument;
using DocumentService.Application.DTOs;
using DocumentService.Application.Queries.GetAllLoanDocuments;
using DocumentService.Application.Queries.GetLoanDocumentById;
using DocumentService.Application.Queries.GetLoanDocumentsByLoanId;

namespace DocumentService.API.MinimalApis;

public static class LoanDocumentEndpoints
{
    public static IEndpointRouteBuilder MapLoanDocumentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/loan-documents")
            .RequireAuthorization()
            .WithTags("LoanDocuments-Minimal");

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllLoanDocumentsQuery(), ct)));

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanDocumentByIdQuery(id), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });

        group.MapGet("/loan/{loanId:long}", async (long loanId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetLoanDocumentsByLoanIdQuery(loanId), ct)));

        group.MapPost("/", async (CreateLoanDocumentCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            return Results.Created($"/api/minimal/loan-documents/{result.Id}", result);
        });

        group.MapPut("/{id:long}", async (long id, MinimalUpdateDocumentRequest req, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new UpdateLoanDocumentCommand(id, req.TypeId, req.ModifiedBy), ct);
            return Results.Ok(result);
        });

        group.MapDelete("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLoanDocumentCommand(id), ct);
            return Results.NoContent();
        });

        return app;
    }
}

internal record MinimalUpdateDocumentRequest(long TypeId, long ModifiedBy);
