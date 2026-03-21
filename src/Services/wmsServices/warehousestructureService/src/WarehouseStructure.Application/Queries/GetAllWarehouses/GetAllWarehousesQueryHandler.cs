using AutoMapper;
using MediatR;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Domain.Interfaces;

namespace WarehouseStructure.Application.Queries.GetAllWarehouses;

public sealed class GetAllWarehousesQueryHandler : IRequestHandler<GetAllWarehousesQuery, IEnumerable<WarehouseDto>>
{
    private readonly IWarehouseRepository _repository;
    private readonly IMapper _mapper;

    public GetAllWarehousesQueryHandler(IWarehouseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WarehouseDto>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
    {
        var warehouses = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<WarehouseDto>>(warehouses);
    }
}
