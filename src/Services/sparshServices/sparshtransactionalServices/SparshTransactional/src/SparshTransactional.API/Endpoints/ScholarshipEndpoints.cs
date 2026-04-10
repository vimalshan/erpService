using MediatR;
using SparshTransactional.Application.Commands;
using SparshTransactional.Application.Queries;

namespace SparshTransactional.API.Endpoints;

public static class ScholarshipEndpoints
{
    public static void MapScholarshipEndpoints(this IEndpointRouteBuilder routes)
    {
        var scholarships = routes.MapGroup("/api/minimal/scholarships")
            .WithTags("Scholarships (Minimal)")
            .RequireAuthorization();

        scholarships.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllScholarshipsQuery(), ct)));

        scholarships.MapGet("/active", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetActiveScholarshipsQuery(), ct)));

        scholarships.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetScholarshipByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        scholarships.MapPost("/", async (CreateScholarshipCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/scholarships/{result.ScholarshipId}", result);
        });

        var applications = routes.MapGroup("/api/minimal/applications")
            .WithTags("Applications (Minimal)")
            .RequireAuthorization();

        applications.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllApplicationsQuery(), ct)));

        applications.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetApplicationByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        applications.MapGet("/status/{status}", async (string status, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetApplicationsByStatusQuery(status), ct)));

        applications.MapGet("/student/{studentId:long}", async (long studentId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetApplicationsByStudentQuery(studentId), ct)));

        applications.MapPost("/", async (SubmitApplicationCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/applications/{result.ApplicationId}", result);
        });

        var disbursements = routes.MapGroup("/api/minimal/disbursements")
            .WithTags("Disbursements (Minimal)")
            .RequireAuthorization();

        disbursements.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllDisbursementsQuery(), ct)));

        disbursements.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDisbursementByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        disbursements.MapGet("/status/{status}", async (string status, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetDisbursementsByStatusQuery(status), ct)));
    }
}
