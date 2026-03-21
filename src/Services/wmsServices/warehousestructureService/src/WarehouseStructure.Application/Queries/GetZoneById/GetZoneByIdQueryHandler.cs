using AutoMapper;
using MediatR;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Domain.Interfaces;

namespace WarehouseStructure.Application.Queries.GetZoneById;

public sealed class GetZoneByIdQueryHandler : IRequestHandler<GetZoneByIdQuery, ZoneDto?>
{
    private readonly IZoneRepository _repository;
    private readonly IMapper _mapper;

    public GetZoneByIdQueryHandler(IZoneRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ZoneDto?> Handle(GetZoneByIdQuery request, CancellationToken cancellationToken)
    {
        var zone = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return zone is null ? null : _mapper.Map<ZoneDto>(zone);
    }
}
