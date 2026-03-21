using MediatR;
using SalesOrderService.Application.SalesOrders.DTOs;

namespace SalesOrderService.Application.SalesOrders.Commands.CreateSalesOrder;

public sealed record CreateSalesOrderCommand(
    string SoNumber,
    int CustomerId,
    int WarehouseId,
    DateOnly OrderDate,
    DateOnly? RequestedDate,
    string? Notes,
    string? CreatedBy,
    IReadOnlyList<CreateSalesOrderLineRequest> Lines)
    : IRequest<SalesOrderDto>;

public sealed record CreateSalesOrderLineRequest(
    int ProductId,
    int LineNumber,
    decimal QuantityOrdered,
    decimal? UnitPrice,
    decimal Discount,
    string? Notes);
