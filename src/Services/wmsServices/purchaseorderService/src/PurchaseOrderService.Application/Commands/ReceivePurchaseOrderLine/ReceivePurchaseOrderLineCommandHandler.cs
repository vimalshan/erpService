using MediatR;
using PurchaseOrderService.Domain.Interfaces;

namespace PurchaseOrderService.Application.Commands.ReceivePurchaseOrderLine;

public class ReceivePurchaseOrderLineCommandHandler : IRequestHandler<ReceivePurchaseOrderLineCommand, Unit>
{
    private readonly IPurchaseOrderRepository _repository;

    public ReceivePurchaseOrderLineCommandHandler(IPurchaseOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(ReceivePurchaseOrderLineCommand request, CancellationToken cancellationToken)
    {
        var po = await _repository.GetByIdAsync(request.PoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Purchase order with ID {request.PoId} not found.");

        po.ReceiveLine(request.LineNumber, request.Quantity);
        await _repository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
