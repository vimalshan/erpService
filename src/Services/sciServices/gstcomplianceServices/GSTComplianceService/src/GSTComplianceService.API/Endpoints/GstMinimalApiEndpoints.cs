using GSTComplianceService.Application.Features.GstMain.Queries;
using MediatR;

namespace GSTComplianceService.API.Endpoints;

public static class GstMinimalApiEndpoints
{
    public static WebApplication MapGstEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/gst-minimal")
            .WithTags("GST Minimal API")
            .RequireAuthorization();

        group.MapGet("/", async (int page, int pageSize, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllGstQuery(page, pageSize), ct);
            return Results.Ok(result);
        })
        .WithSummary("Get all GST registrations (paged)");

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetGstDetailsQuery(id), ct);
            return Results.Ok(result);
        })
        .WithSummary("Get GST registration by ID");

        group.MapGet("/by-pan/{panNo}", async (string panNo, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetGstByPanQuery(panNo), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithSummary("Get GST registration by PAN");

        return app;
    }
}
