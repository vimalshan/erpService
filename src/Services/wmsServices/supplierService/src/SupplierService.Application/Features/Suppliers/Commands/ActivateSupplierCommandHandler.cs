using MediatR;
using SupplierService.Domain.Repositories;

namespace SupplierService.Application.Features.Suppliers.Commands;

public class ActivateSupplierCommandHandler : IRequestHandler<ActivateSupplierCommand, Unit>
{
    private readonly ISupplierRepository _repository;

    public ActivateSupplierCommandHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(ActivateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _repository.GetByIdAsync(request.SupplierId, cancellationToken)
            ?? throw new KeyNotFoundException($"Supplier with ID {request.SupplierId} not found.");

        supplier.Activate();
        await _repository.UpdateAsync(supplier, cancellationToken);
        return Unit.Value;
    }
}
