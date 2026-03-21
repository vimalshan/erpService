using MediatR;
using SalesOrderService.Domain.Exceptions;
using SalesOrderService.Domain.Interfaces;

namespace SalesOrderService.Application.SalesOrders.Commands.ConfirmSalesOrder;

public sealed class ConfirmSalesOrderHandler(IUnitOfWork uow)
    : IRequestHandler<ConfirmSalesOrderCommand>
{
    public async Task Handle(ConfirmSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await uow.SalesOrders.GetByIdAsync(request.SoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales order {request.SoId} not found.");

        order.Confirm();
        uow.SalesOrders.Update(order);
        await uow.SaveChangesAsync(cancellationToken);
    }
}
