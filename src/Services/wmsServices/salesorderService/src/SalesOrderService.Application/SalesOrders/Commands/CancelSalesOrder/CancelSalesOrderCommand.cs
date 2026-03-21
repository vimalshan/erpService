using MediatR;

namespace SalesOrderService.Application.SalesOrders.Commands.CancelSalesOrder;

public sealed record CancelSalesOrderCommand(int SoId, string Reason) : IRequest;
