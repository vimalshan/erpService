using MediatR;
using TourServices.Application.DTOs;
using TourServices.Domain.Exceptions;
using TourServices.Domain.Interfaces;

namespace TourServices.Application.TourPackages.Queries.GetTourPackageById;

public sealed record GetTourPackageByIdQuery(long TourId) : IRequest<TourPackageDto>;

public sealed class GetTourPackageByIdQueryHandler : IRequestHandler<GetTourPackageByIdQuery, TourPackageDto>
{
    private readonly ITourPackageRepository _repository;

    public GetTourPackageByIdQueryHandler(ITourPackageRepository repository) => _repository = repository;

    public async Task<TourPackageDto> Handle(GetTourPackageByIdQuery request, CancellationToken cancellationToken)
    {
        var tour = await _repository.GetByIdAsync(request.TourId, cancellationToken)
            ?? throw new TourNotFoundException(request.TourId);

        return new TourPackageDto(
            tour.TourId, tour.TourName, tour.Destination, tour.StartDate, tour.EndDate,
            tour.TourPackageCost.Amount, tour.MaxParticipants, tour.TourStatus.Code,
            tour.Registrations.Count(r => r.RegistrationStatus == Domain.ValueObjects.RegistrationStatus.Active),
            tour.CreatedBy, tour.CreatedOn, tour.ModifiedBy, tour.ModifiedOn);
    }
}
