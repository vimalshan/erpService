using MediatR;
using SalesOrderService.Application.SalesOrders.DTOs;

namespace SalesOrderService.Application.SalesOrders.Commands.AddOrderLine;

public sealed record AddOrderLineCommand(
    int SoId,
    int ProductId,
    int LineNumber,
    decimal QuantityOrdered,
    decimal? UnitPrice,
    decimal Discount,
    string? Notes)
    : IRequest<SalesOrderLineDto>;
