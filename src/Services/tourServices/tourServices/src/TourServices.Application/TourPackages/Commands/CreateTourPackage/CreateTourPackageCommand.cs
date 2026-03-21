using FluentValidation;
using MediatR;
using TourServices.Application.Common.Interfaces;
using TourServices.Application.DTOs;
using TourServices.Domain.Aggregates;
using TourServices.Domain.Interfaces;

namespace TourServices.Application.TourPackages.Commands.CreateTourPackage;

public sealed record CreateTourPackageCommand(
    string TourName,
    string Destination,
    DateOnly StartDate,
    DateOnly EndDate,
    decimal TotalCost,
    int MaxParticipants,
    long PlannerId) : IRequest<TourPackageDto>;

public sealed class CreateTourPackageCommandValidator : AbstractValidator<CreateTourPackageCommand>
{
    public CreateTourPackageCommandValidator()
    {
        RuleFor(x => x.TourName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Destination).NotEmpty().MaximumLength(100);
        RuleFor(x => x.StartDate).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        RuleFor(x => x.TotalCost).GreaterThan(0);
        RuleFor(x => x.MaxParticipants).GreaterThan(0);
        RuleFor(x => x.PlannerId).GreaterThan(0);
    }
}

public sealed class CreateTourPackageCommandHandler : IRequestHandler<CreateTourPackageCommand, TourPackageDto>
{
    private readonly ITourPackageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTourPackageCommandHandler(ITourPackageRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TourPackageDto> Handle(CreateTourPackageCommand request, CancellationToken cancellationToken)
    {
        var tour = TourPackage.Plan(
            request.TourName,
            request.Destination,
            request.StartDate,
            request.EndDate,
            request.TotalCost,
            request.MaxParticipants,
            request.PlannerId);

        await _repository.AddAsync(tour, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(tour);
    }

    private static TourPackageDto MapToDto(TourPackage t) => new(
        t.TourId, t.TourName, t.Destination, t.StartDate, t.EndDate,
        t.TourPackageCost.Amount, t.MaxParticipants, t.TourStatus.Code,
        t.Registrations.Count(r => r.RegistrationStatus == Domain.ValueObjects.RegistrationStatus.Active),
        t.CreatedBy, t.CreatedOn, t.ModifiedBy, t.ModifiedOn);
}
