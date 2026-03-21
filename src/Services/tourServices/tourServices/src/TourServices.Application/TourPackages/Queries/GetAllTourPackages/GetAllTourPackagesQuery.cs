using MediatR;
using TourServices.Application.DTOs;
using TourServices.Domain.Interfaces;

namespace TourServices.Application.TourPackages.Queries.GetAllTourPackages;

public sealed record GetAllTourPackagesQuery(string? Status = null) : IRequest<IEnumerable<TourPackageDto>>;

public sealed class GetAllTourPackagesQueryHandler : IRequestHandler<GetAllTourPackagesQuery, IEnumerable<TourPackageDto>>
{
    private readonly ITourPackageRepository _repository;

    public GetAllTourPackagesQueryHandler(ITourPackageRepository repository) => _repository = repository;

    public async Task<IEnumerable<TourPackageDto>> Handle(
        GetAllTourPackagesQuery request, CancellationToken cancellationToken)
    {
        var tours = string.IsNullOrWhiteSpace(request.Status)
            ? await _repository.GetAllAsync(cancellationToken)
            : await _repository.GetByStatusAsync(request.Status, cancellationToken);

        return tours.Select(t => new TourPackageDto(
            t.TourId, t.TourName, t.Destination, t.StartDate, t.EndDate,
            t.TourPackageCost.Amount, t.MaxParticipants, t.TourStatus.Code,
            t.Registrations.Count(r => r.RegistrationStatus == Domain.ValueObjects.RegistrationStatus.Active),
            t.CreatedBy, t.CreatedOn, t.ModifiedBy, t.ModifiedOn));
    }
}
