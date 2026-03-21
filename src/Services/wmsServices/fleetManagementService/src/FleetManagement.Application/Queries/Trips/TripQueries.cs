using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Enums;
using FleetManagement.Domain.Interfaces;
using MediatR;

namespace FleetManagement.Application.Queries.Trips;

public record GetTripByIdQuery(int TripId) : IRequest<TripDto?>;
public record GetTripWithStopsQuery(int TripId) : IRequest<TripDto?>;
public record GetAllTripsQuery : IRequest<IReadOnlyList<TripDto>>;
public record GetTripsByStatusQuery(string Status) : IRequest<IReadOnlyList<TripDto>>;

public class GetTripByIdHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetTripByIdQuery, TripDto?>
{
    public async Task<TripDto?> Handle(GetTripByIdQuery request, CancellationToken ct)
    {
        var trip = await uow.Trips.GetByIdAsync(request.TripId, ct);
        return trip is null ? null : mapper.Map<TripDto>(trip);
    }
}

public class GetTripWithStopsHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetTripWithStopsQuery, TripDto?>
{
    public async Task<TripDto?> Handle(GetTripWithStopsQuery request, CancellationToken ct)
    {
        var trip = await uow.Trips.GetWithStopsAsync(request.TripId, ct);
        return trip is null ? null : mapper.Map<TripDto>(trip);
    }
}

public class GetAllTripsHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetAllTripsQuery, IReadOnlyList<TripDto>>
{
    public async Task<IReadOnlyList<TripDto>> Handle(GetAllTripsQuery request, CancellationToken ct)
    {
        var trips = await uow.Trips.GetAllAsync(ct);
        return trips.Select(mapper.Map<TripDto>).ToList();
    }
}

public class GetTripsByStatusHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetTripsByStatusQuery, IReadOnlyList<TripDto>>
{
    public async Task<IReadOnlyList<TripDto>> Handle(GetTripsByStatusQuery request, CancellationToken ct)
    {
        var status = Enum.Parse<TripStatus>(request.Status);
        var trips = await uow.Trips.GetByStatusAsync(status, ct);
        return trips.Select(mapper.Map<TripDto>).ToList();
    }
}
