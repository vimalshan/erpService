using AutoMapper;
using MediatR;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Domain.Entities;
using WarehouseStructure.Domain.Events;
using WarehouseStructure.Domain.Interfaces;

namespace WarehouseStructure.Application.Commands.CreateWarehouse;

public sealed class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, WarehouseDto>
{
    private readonly IWarehouseRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateWarehouseCommandHandler(IWarehouseRepository repository, IMapper mapper, IMediator mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<WarehouseDto> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = _mapper.Map<Warehouse>(request.Dto);
        warehouse.IsActive = true;
        warehouse.CreatedDate = DateTime.UtcNow;
        warehouse.ModifiedDate = DateTime.UtcNow;

        var created = await _repository.AddAsync(warehouse, cancellationToken);

        // Publish after save so the DB-generated WarehouseId is correct
        await _mediator.Publish(new WarehouseCreatedEvent(created.WarehouseId, created.Code, created.Name), cancellationToken);

        return _mapper.Map<WarehouseDto>(created);
    }
}
