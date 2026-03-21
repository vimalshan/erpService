using MediatR;
using TourServices.Application.Common.Interfaces;
using TourServices.Domain.Exceptions;
using TourServices.Domain.Interfaces;

namespace TourServices.Application.TourPackages.Commands.ChangeTourStatus;

public sealed record ActivateTourPackageCommand(long TourId, long UpdatedBy) : IRequest;
public sealed record CancelTourPackageCommand(long TourId, long UpdatedBy) : IRequest;
public sealed record CompleteTourPackageCommand(long TourId, long UpdatedBy) : IRequest;

public sealed class ActivateTourPackageCommandHandler : IRequestHandler<ActivateTourPackageCommand>
{
    private readonly ITourPackageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateTourPackageCommandHandler(ITourPackageRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ActivateTourPackageCommand request, CancellationToken cancellationToken)
    {
        var tour = await _repository.GetByIdAsync(request.TourId, cancellationToken)
            ?? throw new TourNotFoundException(request.TourId);
        tour.Activate(request.UpdatedBy);
        _repository.Update(tour);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

public sealed class CancelTourPackageCommandHandler : IRequestHandler<CancelTourPackageCommand>
{
    private readonly ITourPackageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelTourPackageCommandHandler(ITourPackageRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CancelTourPackageCommand request, CancellationToken cancellationToken)
    {
        var tour = await _repository.GetByIdAsync(request.TourId, cancellationToken)
            ?? throw new TourNotFoundException(request.TourId);
        tour.Cancel(request.UpdatedBy);
        _repository.Update(tour);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

public sealed class CompleteTourPackageCommandHandler : IRequestHandler<CompleteTourPackageCommand>
{
    private readonly ITourPackageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteTourPackageCommandHandler(ITourPackageRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CompleteTourPackageCommand request, CancellationToken cancellationToken)
    {
        var tour = await _repository.GetByIdAsync(request.TourId, cancellationToken)
            ?? throw new TourNotFoundException(request.TourId);
        tour.Complete(request.UpdatedBy);
        _repository.Update(tour);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
