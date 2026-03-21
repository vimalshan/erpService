using MediatR;
using PurchaseOrderService.Domain.Entities;
using PurchaseOrderService.Domain.Interfaces;

namespace PurchaseOrderService.Application.Commands.CreatePurchaseOrder;

public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, int>
{
    private readonly IPurchaseOrderRepository _repository;

    public CreatePurchaseOrderCommandHandler(IPurchaseOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = PurchaseOrder.Create(
            request.PoNumber,
            request.SupplierId,
            request.WarehouseId,
            request.OrderDate,
            request.ExpectedDate,
            request.Notes,
            request.CreatedBy);

        foreach (var line in request.Lines)
        {
            purchaseOrder.AddLine(line.ProductId, line.LineNumber, line.QuantityOrdered, line.UnitPrice, line.Notes);
        }

        await _repository.AddAsync(purchaseOrder, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return purchaseOrder.Id;
    }
}
