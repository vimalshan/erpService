using MediatR;
using SalesOrderService.Application.SalesOrders.DTOs;

namespace SalesOrderService.Application.SalesOrders.Queries.GetAllSalesOrders;

public sealed record GetAllSalesOrdersQuery : IRequest<IEnumerable<SalesOrderSummaryDto>>;
