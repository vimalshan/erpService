using AutoMapper;
using MediatR;
using SalesOrderService.Application.SalesOrders.DTOs;
using SalesOrderService.Domain.Entities;
using SalesOrderService.Domain.Interfaces;

namespace SalesOrderService.Application.SalesOrders.Commands.CreateSalesOrder;

public sealed class CreateSalesOrderHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreateSalesOrderCommand, SalesOrderDto>
{
    public async Task<SalesOrderDto> Handle(CreateSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var order = SalesOrder.Create(
            request.SoNumber,
            request.CustomerId,
            request.WarehouseId,
            request.OrderDate,
            request.RequestedDate,
            request.Notes,
            request.CreatedBy);

        foreach (var line in request.Lines)
            order.AddLine(line.ProductId, line.LineNumber, line.QuantityOrdered,
                line.UnitPrice, line.Discount, line.Notes);

        await uow.SalesOrders.AddAsync(order, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return mapper.Map<SalesOrderDto>(order);
    }
}
