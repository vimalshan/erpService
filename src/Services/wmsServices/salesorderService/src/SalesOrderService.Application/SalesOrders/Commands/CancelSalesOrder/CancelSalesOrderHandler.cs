using MediatR;
using SalesOrderService.Domain.Interfaces;

namespace SalesOrderService.Application.SalesOrders.Commands.CancelSalesOrder;

public sealed class CancelSalesOrderHandler(IUnitOfWork uow)
    : IRequestHandler<CancelSalesOrderCommand>
{
    public async Task Handle(CancelSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await uow.SalesOrders.GetByIdAsync(request.SoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales order {request.SoId} not found.");

        order.Cancel(request.Reason);
        uow.SalesOrders.Update(order);
        await uow.SaveChangesAsync(cancellationToken);
    }
}
