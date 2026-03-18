using MediatR;
using BusServices.Application.DTOs;
using BusServices.Domain.Interfaces;

namespace BusServices.Application.Arrivals.Queries;

public record GetArrivalsByBusQuery(int BusId) : IRequest<IEnumerable<BusArrivalDto>>;

public sealed class GetArrivalsByBusQueryHandler : IRequestHandler<GetArrivalsByBusQuery, IEnumerable<BusArrivalDto>>
{
    private readonly IBusArrivalRepository _repo;

    public GetArrivalsByBusQueryHandler(IBusArrivalRepository repo) => _repo = repo;

    public async Task<IEnumerable<BusArrivalDto>> Handle(GetArrivalsByBusQuery request, CancellationToken ct)
    {
        var arrivals = await _repo.GetByBusIdAsync(request.BusId, ct);
        return arrivals.Select(a => new BusArrivalDto(
            a.ArrivalId, a.BusId, a.ArrivalDate,
            a.ArrivalTime.ToString("HH:mm"), a.Status.Value.ToString(),
            a.Remarks, a.LastModifiedBy, a.LastModifiedOn));
    }
}

public record GetArrivalsByDateQuery(DateTime Date) : IRequest<IEnumerable<BusArrivalDto>>;

public sealed class GetArrivalsByDateQueryHandler : IRequestHandler<GetArrivalsByDateQuery, IEnumerable<BusArrivalDto>>
{
    private readonly IBusArrivalRepository _repo;

    public GetArrivalsByDateQueryHandler(IBusArrivalRepository repo) => _repo = repo;

    public async Task<IEnumerable<BusArrivalDto>> Handle(GetArrivalsByDateQuery request, CancellationToken ct)
    {
        var arrivals = await _repo.GetByDateAsync(request.Date, ct);
        return arrivals.Select(a => new BusArrivalDto(
            a.ArrivalId, a.BusId, a.ArrivalDate,
            a.ArrivalTime.ToString("HH:mm"), a.Status.Value.ToString(),
            a.Remarks, a.LastModifiedBy, a.LastModifiedOn));
    }
}
