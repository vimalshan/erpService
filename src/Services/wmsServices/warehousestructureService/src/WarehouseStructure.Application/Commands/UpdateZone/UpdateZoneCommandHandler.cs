using AutoMapper;
using MediatR;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Domain.Interfaces;

namespace WarehouseStructure.Application.Commands.UpdateZone;

public sealed class UpdateZoneCommandHandler : IRequestHandler<UpdateZoneCommand, ZoneDto>
{
    private readonly IZoneRepository _repository;
    private readonly IMapper _mapper;

    public UpdateZoneCommandHandler(IZoneRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ZoneDto> Handle(UpdateZoneCommand request, CancellationToken cancellationToken)
    {
        var zone = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Zone with Id {request.Id} not found.");

        zone.Name = request.Dto.Name;
        zone.ZoneTypeValue = request.Dto.ZoneType;
        zone.Description = request.Dto.Description;
        zone.IsActive = request.Dto.IsActive;
        zone.ModifiedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(zone, cancellationToken);
        return _mapper.Map<ZoneDto>(zone);
    }
}
