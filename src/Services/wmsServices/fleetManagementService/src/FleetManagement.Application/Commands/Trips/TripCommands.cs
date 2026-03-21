using FleetManagement.Application.DTOs;
using MediatR;

namespace FleetManagement.Application.Commands.Trips;

public record CreateTripCommand(
    string TripNumber, int VehicleId, int DriverId, int? RouteId,
    string? OriginType, int? OriginId,
    string? DestinationType, int? DestinationId,
    string? CreatedBy, List<CreateTripStopCommand>? Stops) : IRequest<TripDto>;

public record CreateTripStopCommand(
    int StopSequence, string? StopType, string? LocationType,
    int? LocationId, string? Address,
    DateTime? PlannedArrival, DateTime? PlannedDeparture);

public record StartTripCommand(int TripId, DateTime? StartTime) : IRequest<TripDto>;

public record CompleteTripCommand(int TripId, DateTime? EndTime) : IRequest<TripDto>;

public record CancelTripCommand(int TripId) : IRequest<TripDto>;
