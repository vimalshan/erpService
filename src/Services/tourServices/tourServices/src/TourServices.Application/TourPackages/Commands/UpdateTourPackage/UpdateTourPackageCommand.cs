using FluentValidation;
using MediatR;
using TourServices.Application.Common.Interfaces;
using TourServices.Application.DTOs;
using TourServices.Domain.Exceptions;
using TourServices.Domain.Interfaces;

namespace TourServices.Application.TourPackages.Commands.UpdateTourPackage;

public sealed record UpdateTourPackageCommand(
    long TourId,
    string TourName,
    string Destination,
    DateOnly StartDate,
    DateOnly EndDate,
    decimal TotalCost,
    int MaxParticipants,
    long UpdatedBy) : IRequest<TourPackageDto>;

public sealed class UpdateTourPackageCommandValidator : AbstractValidator<UpdateTourPackageCommand>
{
    public UpdateTourPackageCommandValidator()
    {
        RuleFor(x => x.TourId).GreaterThan(0);
        RuleFor(x => x.TourName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Destination).NotEmpty().MaximumLength(100);
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        RuleFor(x => x.TotalCost).GreaterThan(0);
        RuleFor(x => x.MaxParticipants).GreaterThan(0);
    }
}

public sealed class UpdateTourPackageCommandHandler : IRequestHandler<UpdateTourPackageCommand, TourPackageDto>
{
    private readonly ITourPackageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTourPackageCommandHandler(ITourPackageRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TourPackageDto> Handle(UpdateTourPackageCommand request, CancellationToken cancellationToken)
    {
        var tour = await _repository.GetByIdAsync(request.TourId, cancellationToken)
            ?? throw new TourNotFoundException(request.TourId);

        tour.Update(request.TourName, request.Destination, request.StartDate,
            request.EndDate, request.TotalCost, request.MaxParticipants, request.UpdatedBy);

        _repository.Update(tour);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TourPackageDto(tour.TourId, tour.TourName, tour.Destination,
            tour.StartDate, tour.EndDate, tour.TourPackageCost.Amount,
            tour.MaxParticipants, tour.TourStatus.Code,
            tour.Registrations.Count(r => r.RegistrationStatus == Domain.ValueObjects.RegistrationStatus.Active),
            tour.CreatedBy, tour.CreatedOn, tour.ModifiedBy, tour.ModifiedOn);
    }
}
