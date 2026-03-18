using MediatR;
using BusServices.Application.DTOs;
using BusServices.Domain.Interfaces;

namespace BusServices.Application.Buses.Queries;

// ─── Get All Buses ───────────────────────────────────────────────────────────

public record GetBusesQuery : IRequest<IEnumerable<BusDto>>;

public sealed class GetBusesQueryHandler : IRequestHandler<GetBusesQuery, IEnumerable<BusDto>>
{
    private readonly IBusRepository _repo;

    public GetBusesQueryHandler(IBusRepository repo) => _repo = repo;

    public async Task<IEnumerable<BusDto>> Handle(GetBusesQuery request, CancellationToken ct)
    {
        var buses = await _repo.GetAllAsync(ct);
        return buses.Select(b => new BusDto(
            b.BusId, b.RegistrationNumber.Value, b.Description,
            b.Capacity, b.CapacityReserved, b.OperatingFrom,
            b.LastModifiedBy, b.LastModifiedOn));
    }
}

// ─── Get Bus By Id ───────────────────────────────────────────────────────────

public record GetBusByIdQuery(int BusId) : IRequest<BusDto?>;

public sealed class GetBusByIdQueryHandler : IRequestHandler<GetBusByIdQuery, BusDto?>
{
    private readonly IBusRepository _repo;

    public GetBusByIdQueryHandler(IBusRepository repo) => _repo = repo;

    public async Task<BusDto?> Handle(GetBusByIdQuery request, CancellationToken ct)
    {
        var bus = await _repo.GetByIdAsync(request.BusId, ct);
        if (bus is null) return null;

        return new BusDto(
            bus.BusId, bus.RegistrationNumber.Value, bus.Description,
            bus.Capacity, bus.CapacityReserved, bus.OperatingFrom,
            bus.LastModifiedBy, bus.LastModifiedOn);
    }
}
