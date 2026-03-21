using AutoMapper;
using MediatR;
using SupplierService.Application.DTOs;
using SupplierService.Domain.Entities;
using SupplierService.Domain.Repositories;
using SupplierService.Domain.ValueObjects;

namespace SupplierService.Application.Features.Suppliers.Commands;

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, SupplierDto>
{
    private readonly ISupplierRepository _repository;
    private readonly IMapper _mapper;

    public CreateSupplierCommandHandler(ISupplierRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SupplierDto> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Supplier;

        if (await _repository.ExistsAsync(dto.Code, cancellationToken))
            throw new InvalidOperationException($"Supplier with code '{dto.Code}' already exists.");

        var address = new Address(
            dto.Address ?? string.Empty,
            dto.City ?? string.Empty,
            dto.State ?? string.Empty,
            dto.Country ?? string.Empty,
            dto.PostalCode ?? string.Empty);

        var supplier = Supplier.Create(dto.Code, dto.Name, dto.ContactPerson, dto.Email, dto.Phone, address);
        var created = await _repository.AddAsync(supplier, cancellationToken);

        return _mapper.Map<SupplierDto>(created);
    }
}
