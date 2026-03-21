using AutoMapper;
using MediatR;
using SalesOrderService.Application.SalesOrders.DTOs;
using SalesOrderService.Domain.Interfaces;

namespace SalesOrderService.Application.SalesOrders.Queries.GetSalesOrdersByCustomer;

public sealed class GetSalesOrdersByCustomerHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetSalesOrdersByCustomerQuery, IEnumerable<SalesOrderSummaryDto>>
{
    public async Task<IEnumerable<SalesOrderSummaryDto>> Handle(GetSalesOrdersByCustomerQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await uow.SalesOrders.GetByCustomerIdAsync(request.CustomerId, cancellationToken);
        return mapper.Map<IEnumerable<SalesOrderSummaryDto>>(orders);
    }
}
