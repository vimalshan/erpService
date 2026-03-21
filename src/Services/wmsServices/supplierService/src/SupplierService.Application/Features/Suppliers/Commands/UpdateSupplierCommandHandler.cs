using AutoMapper;
using MediatR;
using SupplierService.Application.DTOs;
using SupplierService.Domain.Repositories;
using SupplierService.Domain.ValueObjects;

namespace SupplierService.Application.Features.Suppliers.Commands;

public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, SupplierDto>
{
    private readonly ISupplierRepository _repository;
    private readonly IMapper _mapper;

    public UpdateSupplierCommandHandler(ISupplierRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SupplierDto> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _repository.GetByIdAsync(request.SupplierId, cancellationToken)
            ?? throw new KeyNotFoundException($"Supplier with ID {request.SupplierId} not found.");

        var dto = request.Supplier;
        var address = new Address(
            dto.Address ?? string.Empty,
            dto.City ?? string.Empty,
            dto.State ?? string.Empty,
            dto.Country ?? string.Empty,
            dto.PostalCode ?? string.Empty);

        supplier.Update(dto.Name, dto.ContactPerson, dto.Email, dto.Phone, address);
        await _repository.UpdateAsync(supplier, cancellationToken);

        return _mapper.Map<SupplierDto>(supplier);
    }
}
