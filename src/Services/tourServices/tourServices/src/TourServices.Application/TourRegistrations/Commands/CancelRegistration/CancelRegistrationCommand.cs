using MediatR;
using TourServices.Application.Common.Interfaces;
using TourServices.Domain.Exceptions;
using TourServices.Domain.Interfaces;

namespace TourServices.Application.TourRegistrations.Commands.CancelRegistration;

public sealed record CancelRegistrationCommand(long TourId, long RegistrationId, long CancelledBy) : IRequest;

public sealed class CancelRegistrationCommandHandler : IRequestHandler<CancelRegistrationCommand>
{
    private readonly ITourPackageRepository _tourRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelRegistrationCommandHandler(ITourPackageRepository tourRepository, IUnitOfWork unitOfWork)
    {
        _tourRepository = tourRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CancelRegistrationCommand request, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.GetByIdAsync(request.TourId, cancellationToken)
            ?? throw new TourNotFoundException(request.TourId);

        tour.CancelRegistration(request.RegistrationId, request.CancelledBy);
        _tourRepository.Update(tour);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
