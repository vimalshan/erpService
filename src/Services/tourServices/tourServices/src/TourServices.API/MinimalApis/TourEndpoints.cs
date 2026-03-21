using MediatR;
using TourServices.Application.TourPackages.Commands.CreateTourPackage;
using TourServices.Application.TourPackages.Queries.GetAllTourPackages;
using TourServices.Application.TourPackages.Queries.GetTourPackageById;
using TourServices.Application.TourRegistrations.Commands.RegisterParticipant;
using TourServices.Application.TourRegistrations.Queries.GetRegistrationsByTour;

namespace TourServices.API.MinimalApis;

public static class TourEndpoints
{
    public static IEndpointRouteBuilder MapTourEndpoints(this IEndpointRouteBuilder app)
    {
        var tours = app.MapGroup("/api/v2/tours").RequireAuthorization().WithTags("Tours v2");

        tours.MapGet("/", async (IMediator mediator, string? status, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetAllTourPackagesQuery(status), ct)));

        tours.MapGet("/{id:long}", async (IMediator mediator, long id, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTourPackageByIdQuery(id), ct);
            return Results.Ok(result);
        });

        tours.MapPost("/", async (IMediator mediator, CreateTourPackageCommand command, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/tours/{result.TourId}", result);
        });

        tours.MapGet("/{tourId:long}/registrations", async (IMediator mediator, long tourId, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetRegistrationsByTourQuery(tourId), ct)));

        tours.MapPost("/{tourId:long}/registrations", async (
            IMediator mediator, long tourId, RegisterParticipantCommand command, CancellationToken ct) =>
        {
            var result = await mediator.Send(command with { TourId = tourId }, ct);
            return Results.Created($"/api/v2/tours/{tourId}/registrations/{result.RegistrationId}", result);
        });

        return app;
    }
}
