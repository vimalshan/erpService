using MediatR;
using BusServices.Application.DTOs;
using BusServices.Domain.Interfaces;
using BusServices.Domain.Entities;

namespace BusServices.Application.Buses.Commands;

// ─── Register Bus ────────────────────────────────────────────────────────────

public record RegisterBusCommand(
    string RegistrationNumber,
    string? Description,
    int Capacity,
    long RegisteredBy) : IRequest<BusDto>;

public sealed class RegisterBusCommandHandler : IRequestHandler<RegisterBusCommand, BusDto>
{
    private readonly IBusRepository _repo;

    public RegisterBusCommandHandler(IBusRepository repo) => _repo = repo;

    public async Task<BusDto> Handle(RegisterBusCommand request, CancellationToken ct)
    {
        if (await _repo.RegistrationExistsAsync(request.RegistrationNumber, ct))
            throw new InvalidOperationException($"Bus '{request.RegistrationNumber}' is already registered.");

        int nextId = await _repo.GetNextIdAsync(ct);
        var bus = Bus.Register(nextId, request.RegistrationNumber, request.Description, request.Capacity, request.RegisteredBy);

        await _repo.AddAsync(bus, ct);
        await _repo.SaveChangesAsync(ct);

        return MapToDto(bus);
    }

    private static BusDto MapToDto(Bus b) => new(
        b.BusId, b.RegistrationNumber.Value, b.Description,
        b.Capacity, b.CapacityReserved, b.OperatingFrom,
        b.LastModifiedBy, b.LastModifiedOn);
}

// ─── Update Bus ──────────────────────────────────────────────────────────────

public record UpdateBusCommand(
    int BusId,
    string? Description,
    int Capacity,
    long ModifiedBy) : IRequest<BusDto>;

public sealed class UpdateBusCommandHandler : IRequestHandler<UpdateBusCommand, BusDto>
{
    private readonly IBusRepository _repo;

    public UpdateBusCommandHandler(IBusRepository repo) => _repo = repo;

    public async Task<BusDto> Handle(UpdateBusCommand request, CancellationToken ct)
    {
        var bus = await _repo.GetByIdAsync(request.BusId, ct)
            ?? throw new KeyNotFoundException($"Bus {request.BusId} not found.");

        bus.UpdateDetails(request.Description, request.Capacity, request.ModifiedBy);
        _repo.Update(bus);
        await _repo.SaveChangesAsync(ct);

        return new BusDto(bus.BusId, bus.RegistrationNumber.Value, bus.Description,
            bus.Capacity, bus.CapacityReserved, bus.OperatingFrom,
            bus.LastModifiedBy, bus.LastModifiedOn);
    }
}
