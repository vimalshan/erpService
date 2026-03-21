using FluentValidation;
using MediatR;
using TourServices.Application.Common.Interfaces;
using TourServices.Application.DTOs;
using TourServices.Domain.Exceptions;
using TourServices.Domain.Interfaces;

namespace TourServices.Application.TourRegistrations.Commands.RegisterParticipant;

public sealed record RegisterParticipantCommand(
    long TourId,
    long ParticipantId,
    DateOnly RegistrationDate,
    long RegisteredBy) : IRequest<TourRegistrationDto>;

public sealed class RegisterParticipantCommandValidator : AbstractValidator<RegisterParticipantCommand>
{
    public RegisterParticipantCommandValidator()
    {
        RuleFor(x => x.TourId).GreaterThan(0);
        RuleFor(x => x.ParticipantId).GreaterThan(0);
        RuleFor(x => x.RegisteredBy).GreaterThan(0);
    }
}

public sealed class RegisterParticipantCommandHandler : IRequestHandler<RegisterParticipantCommand, TourRegistrationDto>
{
    private readonly ITourPackageRepository _tourRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterParticipantCommandHandler(ITourPackageRepository tourRepository, IUnitOfWork unitOfWork)
    {
        _tourRepository = tourRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TourRegistrationDto> Handle(RegisterParticipantCommand request, CancellationToken cancellationToken)
    {
        var tour = await _tourRepository.GetByIdAsync(request.TourId, cancellationToken)
            ?? throw new TourNotFoundException(request.TourId);

        var registration = tour.RegisterParticipant(
            request.ParticipantId, request.RegistrationDate, request.RegisteredBy);

        _tourRepository.Update(tour);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TourRegistrationDto(
            registration.RegistrationId, registration.TourId, tour.TourName,
            registration.ParticipantId, registration.RegistrationDate,
            registration.RegistrationStatus.Code, registration.CreatedBy, registration.CreatedOn);
    }
}
