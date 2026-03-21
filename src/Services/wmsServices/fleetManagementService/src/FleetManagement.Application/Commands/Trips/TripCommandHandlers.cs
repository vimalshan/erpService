using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Events;
using FleetManagement.Domain.Interfaces;
using MediatR;

namespace FleetManagement.Application.Commands.Trips;

public class CreateTripHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CreateTripCommand, TripDto>
{
    public async Task<TripDto> Handle(CreateTripCommand request, CancellationToken ct)
    {
        var trip = new Trip
        {
            TripNumber = request.TripNumber,
            VehicleId = request.VehicleId,
            DriverId = request.DriverId,
            RouteId = request.RouteId,
            OriginType = request.OriginType,
            OriginId = request.OriginId,
            DestinationType = request.DestinationType,
            DestinationId = request.DestinationId,
            CreatedBy = request.CreatedBy
        };

        if (request.Stops is { Count: > 0 })
        {
            foreach (var s in request.Stops)
            {
                trip.Stops.Add(new TripStop
                {
                    StopSequence = s.StopSequence,
                    StopType = s.StopType,
                    LocationType = s.LocationType,
                    LocationId = s.LocationId,
                    Address = s.Address,
                    PlannedArrival = s.PlannedArrival,
                    PlannedDeparture = s.PlannedDeparture
                });
            }
        }

        trip.AddDomainEvent(new TripCreatedEvent(trip.TripId, trip.TripNumber, trip.VehicleId, trip.DriverId));
        await uow.Trips.AddAsync(trip, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<TripDto>(trip);
    }
}

public class StartTripHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<StartTripCommand, TripDto>
{
    public async Task<TripDto> Handle(StartTripCommand request, CancellationToken ct)
    {
        var trip = await uow.Trips.GetByIdAsync(request.TripId, ct)
            ?? throw new KeyNotFoundException($"Trip {request.TripId} not found.");
        trip.Start(request.StartTime);
        await uow.Trips.UpdateAsync(trip, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<TripDto>(trip);
    }
}

public class CompleteTripHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CompleteTripCommand, TripDto>
{
    public async Task<TripDto> Handle(CompleteTripCommand request, CancellationToken ct)
    {
        var trip = await uow.Trips.GetByIdAsync(request.TripId, ct)
            ?? throw new KeyNotFoundException($"Trip {request.TripId} not found.");
        trip.Complete(request.EndTime);
        await uow.Trips.UpdateAsync(trip, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<TripDto>(trip);
    }
}

public class CancelTripHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CancelTripCommand, TripDto>
{
    public async Task<TripDto> Handle(CancelTripCommand request, CancellationToken ct)
    {
        var trip = await uow.Trips.GetByIdAsync(request.TripId, ct)
            ?? throw new KeyNotFoundException($"Trip {request.TripId} not found.");
        trip.Cancel();
        await uow.Trips.UpdateAsync(trip, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<TripDto>(trip);
    }
}
