using MediatR;
using PurchaseOrderService.Domain.Interfaces;

namespace PurchaseOrderService.Application.Commands.CancelPurchaseOrder;

public class CancelPurchaseOrderCommandHandler : IRequestHandler<CancelPurchaseOrderCommand, Unit>
{
    private readonly IPurchaseOrderRepository _repository;

    public CancelPurchaseOrderCommandHandler(IPurchaseOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(CancelPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var po = await _repository.GetByIdAsync(request.PoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Purchase order with ID {request.PoId} not found.");

        po.Cancel();
        await _repository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
