using MediatR;
using SalesOrderService.Application.SalesOrders.DTOs;

namespace SalesOrderService.Application.SalesOrders.Queries.GetSalesOrdersByCustomer;

public sealed record GetSalesOrdersByCustomerQuery(int CustomerId) : IRequest<IEnumerable<SalesOrderSummaryDto>>;
