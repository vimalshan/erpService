using MediatR;
using TourServices.Application.DTOs;
using TourServices.Application.TourPackages.Commands.ChangeTourStatus;
using TourServices.Application.TourPackages.Commands.CreateTourPackage;
using TourServices.Application.TourPackages.Commands.UpdateTourPackage;
using TourServices.Application.TourRegistrations.Commands.CancelRegistration;
using TourServices.Application.TourRegistrations.Commands.RegisterParticipant;

namespace TourServices.API.GraphQL;

public sealed class Mutation
{
    public async Task<TourPackageDto> CreateTourPackageAsync(
        [Service] IMediator mediator,
        CreateTourPackageInput input,
        CancellationToken ct)
        => await mediator.Send(new CreateTourPackageCommand(
            input.TourName, input.Destination, input.StartDate, input.EndDate,
            input.TotalCost, input.MaxParticipants, input.PlannerId), ct);

    public async Task<TourPackageDto> UpdateTourPackageAsync(
        [Service] IMediator mediator,
        UpdateTourPackageInput input,
        CancellationToken ct)
        => await mediator.Send(new UpdateTourPackageCommand(
            input.TourId, input.TourName, input.Destination, input.StartDate, input.EndDate,
            input.TotalCost, input.MaxParticipants, input.UpdatedBy), ct);

    public async Task<bool> ActivateTourPackageAsync(
        [Service] IMediator mediator, long tourId, long updatedBy, CancellationToken ct)
    {
        await mediator.Send(new ActivateTourPackageCommand(tourId, updatedBy), ct);
        return true;
    }

    public async Task<bool> CancelTourPackageAsync(
        [Service] IMediator mediator, long tourId, long updatedBy, CancellationToken ct)
    {
        await mediator.Send(new CancelTourPackageCommand(tourId, updatedBy), ct);
        return true;
    }

    public async Task<TourRegistrationDto> RegisterParticipantAsync(
        [Service] IMediator mediator,
        RegisterParticipantInput input,
        CancellationToken ct)
        => await mediator.Send(new RegisterParticipantCommand(
            input.TourId, input.ParticipantId, input.RegistrationDate, input.RegisteredBy), ct);

    public async Task<bool> CancelRegistrationAsync(
        [Service] IMediator mediator, long tourId, long registrationId, long cancelledBy, CancellationToken ct)
    {
        await mediator.Send(new CancelRegistrationCommand(tourId, registrationId, cancelledBy), ct);
        return true;
    }
}

public record CreateTourPackageInput(
    string TourName, string Destination,
    DateOnly StartDate, DateOnly EndDate,
    decimal TotalCost, int MaxParticipants, long PlannerId);

public record UpdateTourPackageInput(
    long TourId, string TourName, string Destination,
    DateOnly StartDate, DateOnly EndDate,
    decimal TotalCost, int MaxParticipants, long UpdatedBy);

public record RegisterParticipantInput(
    long TourId, long ParticipantId, DateOnly RegistrationDate, long RegisteredBy);
