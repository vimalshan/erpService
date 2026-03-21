using AutoMapper;
using MediatR;
using SalesOrderService.Application.SalesOrders.DTOs;
using SalesOrderService.Domain.Interfaces;

namespace SalesOrderService.Application.SalesOrders.Commands.AddOrderLine;

public sealed class AddOrderLineHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<AddOrderLineCommand, SalesOrderLineDto>
{
    public async Task<SalesOrderLineDto> Handle(AddOrderLineCommand request, CancellationToken cancellationToken)
    {
        var order = await uow.SalesOrders.GetByIdAsync(request.SoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales order {request.SoId} not found.");

        var line = order.AddLine(request.ProductId, request.LineNumber,
            request.QuantityOrdered, request.UnitPrice, request.Discount, request.Notes);

        uow.SalesOrders.Update(order);
        await uow.SaveChangesAsync(cancellationToken);

        return mapper.Map<SalesOrderLineDto>(line);
    }
}
