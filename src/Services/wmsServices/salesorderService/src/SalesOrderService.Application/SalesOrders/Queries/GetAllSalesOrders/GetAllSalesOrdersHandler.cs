using AutoMapper;
using MediatR;
using SalesOrderService.Application.SalesOrders.DTOs;
using SalesOrderService.Domain.Interfaces;

namespace SalesOrderService.Application.SalesOrders.Queries.GetAllSalesOrders;

public sealed class GetAllSalesOrdersHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllSalesOrdersQuery, IEnumerable<SalesOrderSummaryDto>>
{
    public async Task<IEnumerable<SalesOrderSummaryDto>> Handle(GetAllSalesOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await uow.SalesOrders.GetAllAsync(cancellationToken);
        return mapper.Map<IEnumerable<SalesOrderSummaryDto>>(orders);
    }
}
