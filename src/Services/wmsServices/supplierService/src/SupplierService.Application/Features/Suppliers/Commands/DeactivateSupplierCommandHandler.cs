using MediatR;
using SupplierService.Domain.Repositories;

namespace SupplierService.Application.Features.Suppliers.Commands;

public class DeactivateSupplierCommandHandler : IRequestHandler<DeactivateSupplierCommand, Unit>
{
    private readonly ISupplierRepository _repository;

    public DeactivateSupplierCommandHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeactivateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _repository.GetByIdAsync(request.SupplierId, cancellationToken)
            ?? throw new KeyNotFoundException($"Supplier with ID {request.SupplierId} not found.");

        supplier.Deactivate();
        await _repository.UpdateAsync(supplier, cancellationToken);
        return Unit.Value;
    }
}
