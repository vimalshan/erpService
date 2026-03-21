using MediatR;
using TourServices.Application.DTOs;
using TourServices.Domain.Interfaces;

namespace TourServices.Application.TourRegistrations.Queries.GetRegistrationsByTour;

public sealed record GetRegistrationsByTourQuery(long TourId) : IRequest<IEnumerable<TourRegistrationDto>>;

public sealed class GetRegistrationsByTourQueryHandler
    : IRequestHandler<GetRegistrationsByTourQuery, IEnumerable<TourRegistrationDto>>
{
    private readonly ITourRegistrationRepository _repository;
    private readonly ITourPackageRepository _tourRepository;

    public GetRegistrationsByTourQueryHandler(
        ITourRegistrationRepository repository, ITourPackageRepository tourRepository)
    {
        _repository = repository;
        _tourRepository = tourRepository;
    }

    public async Task<IEnumerable<TourRegistrationDto>> Handle(
        GetRegistrationsByTourQuery request, CancellationToken cancellationToken)
    {
        var registrations = await _repository.GetByTourIdAsync(request.TourId, cancellationToken);
        var tour = await _tourRepository.GetByIdAsync(request.TourId, cancellationToken);
        var tourName = tour?.TourName ?? string.Empty;

        return registrations.Select(r => new TourRegistrationDto(
            r.RegistrationId, r.TourId, tourName, r.ParticipantId,
            r.RegistrationDate, r.RegistrationStatus.Code, r.CreatedBy, r.CreatedOn));
    }
}
