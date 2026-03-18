using MediatR;
using BusServices.Application.DTOs;
using BusServices.Domain.Entities;
using BusServices.Domain.Interfaces;

namespace BusServices.Application.Arrivals.Commands;

// ─── Record Arrival ───────────────────────────────────────────────────────────

public record RecordArrivalCommand(
    int BusId,
    DateTime ArrivalDate,
    TimeOnly ArrivalTime,
    char Status,
    string? Remarks,
    long RecordedBy) : IRequest<BusArrivalDto>;

public sealed class RecordArrivalCommandHandler : IRequestHandler<RecordArrivalCommand, BusArrivalDto>
{
    private readonly IBusRepository _busRepo;
    private readonly IBusArrivalRepository _arrivalRepo;

    public RecordArrivalCommandHandler(IBusRepository busRepo, IBusArrivalRepository arrivalRepo)
    {
        _busRepo = busRepo;
        _arrivalRepo = arrivalRepo;
    }

    public async Task<BusArrivalDto> Handle(RecordArrivalCommand request, CancellationToken ct)
    {
        var bus = await _busRepo.GetByIdAsync(request.BusId, ct)
            ?? throw new KeyNotFoundException($"Bus {request.BusId} not found.");

        long nextId = await _arrivalRepo.GetNextIdAsync(ct);
        var arrival = bus.RecordArrival(nextId, request.ArrivalDate, request.ArrivalTime,
            request.Status, request.Remarks, request.RecordedBy);

        await _arrivalRepo.AddAsync(arrival, ct);
        await _arrivalRepo.SaveChangesAsync(ct);

        return new BusArrivalDto(arrival.ArrivalId, arrival.BusId, arrival.ArrivalDate,
            arrival.ArrivalTime.ToString("HH:mm"), arrival.Status.Value.ToString(),
            arrival.Remarks, arrival.LastModifiedBy, arrival.LastModifiedOn);
    }
}
