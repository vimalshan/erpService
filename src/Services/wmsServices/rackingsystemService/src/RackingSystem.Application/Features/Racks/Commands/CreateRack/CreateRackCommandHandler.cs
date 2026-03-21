using MediatR;
using RackingSystem.Application.Features.Racks.DTOs;
using RackingSystem.Domain.Entities;
using RackingSystem.Domain.Interfaces;

namespace RackingSystem.Application.Features.Racks.Commands.CreateRack;

public sealed class CreateRackCommandHandler : IRequestHandler<CreateRackCommand, RackDto>
{
    private readonly IUnitOfWork _uow;

    public CreateRackCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<RackDto> Handle(CreateRackCommand request, CancellationToken ct)
    {
        if (await _uow.Racks.ExistsAsync(request.ZoneId, request.Code, ct))
            throw new InvalidOperationException($"Rack with code '{request.Code}' already exists in zone {request.ZoneId}.");

        var rack = Rack.Create(request.ZoneId, request.Code, request.RackType, request.MaxLoadWeight);
        await _uow.Racks.AddAsync(rack, ct);
        await _uow.SaveChangesAsync(ct);

        return MapToDto(rack);
    }

    internal static RackDto MapToDto(Rack r) => new(
        r.Id, r.ZoneId, r.Code, r.RackType, r.MaxLoadWeight, r.IsActive,
        r.CreatedDate, r.ModifiedDate,
        r.Shelves.Select(s => new ShelfSummaryDto(s.Id, s.ShelfLevel, s.ShelfPosition, s.Code, s.IsActive))
    );
}
