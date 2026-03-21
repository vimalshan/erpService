using MediatR;
using TourServices.Application.DTOs;
using TourServices.Application.TourPackages.Queries.GetAllTourPackages;
using TourServices.Application.TourPackages.Queries.GetTourPackageById;
using TourServices.Application.TourRegistrations.Queries.GetRegistrationsByTour;

namespace TourServices.API.GraphQL;

public sealed class Query
{
    public async Task<IEnumerable<TourPackageDto>> GetTourPackagesAsync(
        [Service] IMediator mediator, string? status, CancellationToken ct)
        => await mediator.Send(new GetAllTourPackagesQuery(status), ct);

    public async Task<TourPackageDto> GetTourPackageAsync(
        [Service] IMediator mediator, long id, CancellationToken ct)
        => await mediator.Send(new GetTourPackageByIdQuery(id), ct);

    public async Task<IEnumerable<TourRegistrationDto>> GetRegistrationsByTourAsync(
        [Service] IMediator mediator, long tourId, CancellationToken ct)
        => await mediator.Send(new GetRegistrationsByTourQuery(tourId), ct);
}
