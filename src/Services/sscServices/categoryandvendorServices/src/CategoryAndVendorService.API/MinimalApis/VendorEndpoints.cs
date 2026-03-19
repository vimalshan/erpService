using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Application.VendorDocuments.Commands;
using CategoryAndVendorService.Application.VendorDocuments.Queries;
using CategoryAndVendorService.Application.SupportDocuments.Commands;
using CategoryAndVendorService.Application.SupportDocuments.Queries;
using MediatR;

namespace CategoryAndVendorService.API.MinimalApis;

public static class VendorEndpoints
{
    public static IEndpointRouteBuilder MapVendorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/vendor-documents").RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllVendorDocumentsQuery(), ct)));

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetVendorDocumentByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapGet("/by-vendor/{vendorId:long}", async (long vendorId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetVendorDocumentsByVendorIdQuery(vendorId), ct)));

        group.MapPost("/", async (CreateVendorDocumentCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/vendor-documents/{result.VndDocId}", result);
        });

        group.MapPost("/{id:long}/approve", async (long id, ApproveVendorDocumentCommand command, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(command, ct)));

        group.MapPost("/{id:long}/reject", async (long id, RejectVendorDocumentCommand command, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(command, ct)));

        // Support Documents
        var supGroup = app.MapGroup("/api/v2/support-documents").RequireAuthorization();

        supGroup.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllSupportDocumentsQuery(), ct)));

        supGroup.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSupportDocumentByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        supGroup.MapPost("/", async (CreateSupportDocumentCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/support-documents/{result.DocId}", result);
        });

        return app;
    }
}
