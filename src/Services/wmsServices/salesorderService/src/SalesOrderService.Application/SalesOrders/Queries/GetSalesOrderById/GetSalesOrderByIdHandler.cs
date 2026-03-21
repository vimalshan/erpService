using AutoMapper;
using MediatR;
using SalesOrderService.Application.SalesOrders.DTOs;
using SalesOrderService.Domain.Interfaces;

namespace SalesOrderService.Application.SalesOrders.Queries.GetSalesOrderById;

public sealed class GetSalesOrderByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetSalesOrderByIdQuery, SalesOrderDto?>
{
    public async Task<SalesOrderDto?> Handle(GetSalesOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await uow.SalesOrders.GetByIdAsync(request.SoId, cancellationToken);
        return order is null ? null : mapper.Map<SalesOrderDto>(order);
    }
}
