using MediatR;
using WMTransactional.Application.DTOs;

namespace WMTransactional.Application.Commands.CreateSalesOrder;

public record CreateSalesOrderCommand : IRequest<SalesOrderDto>
{
    public string SoNumber { get; init; } = null!;
    public int CustomerId { get; init; }
    public DateTime? RequestedDate { get; init; }
    public string? Notes { get; init; }
    public string? CreatedBy { get; init; }
    public List<CreateSalesOrderLineItem> Lines { get; init; } = [];
}

public record CreateSalesOrderLineItem
{
    public int ProductId { get; init; }
    public decimal QuantityOrdered { get; init; }
    public decimal? UnitPrice { get; init; }
    public string? Notes { get; init; }
}
