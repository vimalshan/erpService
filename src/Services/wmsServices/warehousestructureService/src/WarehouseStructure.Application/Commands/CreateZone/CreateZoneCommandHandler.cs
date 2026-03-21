using AutoMapper;
using MediatR;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Domain.Entities;
using WarehouseStructure.Domain.Interfaces;

namespace WarehouseStructure.Application.Commands.CreateZone;

public sealed class CreateZoneCommandHandler : IRequestHandler<CreateZoneCommand, ZoneDto>
{
    private readonly IZoneRepository _zoneRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IMapper _mapper;

    public CreateZoneCommandHandler(IZoneRepository zoneRepository, IWarehouseRepository warehouseRepository, IMapper mapper)
    {
        _zoneRepository = zoneRepository;
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
    }

    public async Task<ZoneDto> Handle(CreateZoneCommand request, CancellationToken cancellationToken)
    {
        if (!await _warehouseRepository.ExistsAsync(request.Dto.WarehouseId, cancellationToken))
            throw new KeyNotFoundException($"Warehouse with Id {request.Dto.WarehouseId} not found.");

        var zone = _mapper.Map<Zone>(request.Dto);
        zone.IsActive = true;
        zone.CreatedDate = DateTime.UtcNow;
        zone.ModifiedDate = DateTime.UtcNow;

        var created = await _zoneRepository.AddAsync(zone, cancellationToken);
        return _mapper.Map<ZoneDto>(created);
    }
}
