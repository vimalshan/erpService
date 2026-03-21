using MediatR;
using PurchaseOrderService.Domain.Interfaces;

namespace PurchaseOrderService.Application.Commands.ConfirmPurchaseOrder;

public class ConfirmPurchaseOrderCommandHandler : IRequestHandler<ConfirmPurchaseOrderCommand, Unit>
{
    private readonly IPurchaseOrderRepository _repository;

    public ConfirmPurchaseOrderCommandHandler(IPurchaseOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(ConfirmPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var po = await _repository.GetByIdAsync(request.PoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Purchase order with ID {request.PoId} not found.");

        po.Confirm();
        await _repository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
