using AutoMapper;
using MediatR;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Domain.Interfaces;

namespace WarehouseStructure.Application.Queries.GetWarehouseById;

public sealed class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQuery, WarehouseDto?>
{
    private readonly IWarehouseRepository _repository;
    private readonly IMapper _mapper;

    public GetWarehouseByIdQueryHandler(IWarehouseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<WarehouseDto?> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
    {
        var warehouse = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return warehouse is null ? null : _mapper.Map<WarehouseDto>(warehouse);
    }
}
