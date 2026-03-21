using MediatR;
using RackingSystem.Application.Features.Racks.Commands.CreateRack;
using RackingSystem.Application.Features.Racks.DTOs;
using RackingSystem.Domain.Interfaces;

namespace RackingSystem.Application.Features.Racks.Queries.GetRacks;

public sealed class GetRacksQueryHandler : IRequestHandler<GetRacksQuery, IEnumerable<RackDto>>
{
    private readonly IUnitOfWork _uow;

    public GetRacksQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<RackDto>> Handle(GetRacksQuery request, CancellationToken ct)
    {
        var racks = request.ZoneId.HasValue
            ? await _uow.Racks.GetByZoneIdAsync(request.ZoneId.Value, ct)
            : await _uow.Racks.GetAllAsync(ct);

        return racks.Select(CreateRackCommandHandler.MapToDto);
    }
}
