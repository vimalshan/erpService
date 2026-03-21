using AutoMapper;
using MediatR;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Domain.Interfaces;

namespace WarehouseStructure.Application.Queries.GetAllZones;

public sealed class GetAllZonesQueryHandler : IRequestHandler<GetAllZonesQuery, IEnumerable<ZoneDto>>
{
    private readonly IZoneRepository _repository;
    private readonly IMapper _mapper;

    public GetAllZonesQueryHandler(IZoneRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ZoneDto>> Handle(GetAllZonesQuery request, CancellationToken cancellationToken)
    {
        var zones = request.WarehouseId.HasValue
            ? await _repository.GetByWarehouseIdAsync(request.WarehouseId.Value, cancellationToken)
            : await _repository.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ZoneDto>>(zones);
    }
}
