using MediatR;

namespace SalesOrderService.Application.SalesOrders.Commands.ConfirmSalesOrder;

public sealed record ConfirmSalesOrderCommand(int SoId) : IRequest;
