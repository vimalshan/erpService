using AutoMapper;
using MediatR;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Domain.Interfaces;

namespace WarehouseStructure.Application.Commands.UpdateWarehouse;

public sealed class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, WarehouseDto>
{
    private readonly IWarehouseRepository _repository;
    private readonly IMapper _mapper;

    public UpdateWarehouseCommandHandler(IWarehouseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<WarehouseDto> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Warehouse with Id {request.Id} not found.");

        warehouse.Name = request.Dto.Name;
        warehouse.AddressLine = request.Dto.Address;
        warehouse.City = request.Dto.City;
        warehouse.State = request.Dto.State;
        warehouse.Country = request.Dto.Country;
        warehouse.PostalCode = request.Dto.PostalCode;
        warehouse.Phone = request.Dto.Phone;
        warehouse.Email = request.Dto.Email;
        warehouse.IsActive = request.Dto.IsActive;
        warehouse.ModifiedDate = DateTime.UtcNow;

        warehouse.RaiseUpdatedEvent();
        await _repository.UpdateAsync(warehouse, cancellationToken);

        return _mapper.Map<WarehouseDto>(warehouse);
    }
}
