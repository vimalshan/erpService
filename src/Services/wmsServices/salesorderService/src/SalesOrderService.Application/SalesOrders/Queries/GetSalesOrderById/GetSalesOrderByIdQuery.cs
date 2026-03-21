using MediatR;
using SalesOrderService.Application.SalesOrders.DTOs;

namespace SalesOrderService.Application.SalesOrders.Queries.GetSalesOrderById;

public sealed record GetSalesOrderByIdQuery(int SoId) : IRequest<SalesOrderDto?>;
