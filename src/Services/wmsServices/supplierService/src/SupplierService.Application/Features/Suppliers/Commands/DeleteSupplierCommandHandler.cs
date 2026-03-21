using MediatR;
using SupplierService.Domain.Repositories;

namespace SupplierService.Application.Features.Suppliers.Commands;

public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, Unit>
{
    private readonly ISupplierRepository _repository;

    public DeleteSupplierCommandHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _repository.GetByIdAsync(request.SupplierId, cancellationToken)
            ?? throw new KeyNotFoundException($"Supplier with ID {request.SupplierId} not found.");

        await _repository.DeleteAsync(request.SupplierId, cancellationToken);
        return Unit.Value;
    }
}
