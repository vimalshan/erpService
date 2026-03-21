using AutoMapper;
using MediatR;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Domain.Entities;
using WarehouseStructure.Domain.Interfaces;

namespace WarehouseStructure.Application.Commands.CreateWarehouse;

public sealed class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, WarehouseDto>
{
    private readonly IWarehouseRepository _repository;
    private readonly IMapper _mapper;

    public CreateWarehouseCommandHandler(IWarehouseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<WarehouseDto> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = _mapper.Map<Warehouse>(request.Dto);
        warehouse.IsActive = true;
        warehouse.CreatedDate = DateTime.UtcNow;
        warehouse.ModifiedDate = DateTime.UtcNow;

        var created = await _repository.AddAsync(warehouse, cancellationToken);
        created.RaiseCreatedEvent();

        return _mapper.Map<WarehouseDto>(created);
    }
}
